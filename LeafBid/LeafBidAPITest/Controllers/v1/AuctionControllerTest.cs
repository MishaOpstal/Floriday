using System.Reflection;
using System.Security.Claims;
using LeafBidAPI.Controllers.v1;
using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.Enums;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LeafBidAPITest.Controllers.v1;

public sealed class AuctionControllerTest
{
    private static ApplicationDbContext CreateDbContext()
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        ApplicationDbContext dbContext = new(options);

        // Ensures IdentityDbContext tables & model are initialized for this in-memory database instance.
        dbContext.Database.EnsureCreated();

        return dbContext;
    }

    private static Mock<UserManager<User>> CreateUserManagerMock()
    {
        Mock<IUserStore<User>> store = new();
        
        // mock requires the nulls as these are in 'UserManager<TUser>', these are for objects used under the hood by IdentityFramework
        Mock<UserManager<User>> userManager = new( 
            store.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        );

        return userManager;
    }

    private static AuctionController CreateController(
        ApplicationDbContext dbContext,
        UserManager<User> userManager,
        ClaimsPrincipal? user = null) // a claimsPrincipal is a .NET object that represents who the current user is
    {
        // creates a controller with the mock database and user manager
        AuctionController controller = new(dbContext, userManager);
        
        // prepare a mock HTTP context and assign a user
        // if no user is provided, the controller will return a null user
        DefaultHttpContext httpContext = new()
        {
            User = user ?? new ClaimsPrincipal(new ClaimsIdentity())
        };
        
        // assign the mock HTTP context to the controller
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    // helper method to get the message from a JsonResult
    private static string GetJsonMessage(JsonResult json)
    {
        object? value = json.Value;
        Assert.NotNull(value);

        PropertyInfo? messagePropertyInfo = value.GetType().GetProperty("Message");
        Assert.NotNull(messagePropertyInfo);

        object? messageValue = messagePropertyInfo.GetValue(value);
        Assert.NotNull(messageValue);

        return (string)messageValue;
    }

    private static Product CreateValidProduct(int id, string userId, int stock)
    {
        Product product = new()
        {
            Id = id,
            Name = "Test Product",
            Description = "Test Description",
            MinPrice = 1.00m,
            MaxPrice = 2.00m,
            Weight = 1.0,
            Species = "Rose",
            Region = "NL",
            Stock = stock,
            HarvestedAt = DateTime.UtcNow,
            UserId = userId
        };

        return product;
    }

    [Fact]
    public async Task CreateAuction_WhenCurrentUserIsNull_Returns401_AndDefaultMessage()
    {
        // Arrange
        await using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();

        // returns null user when GetUserAsync is called with any ClaimsPrincipal
        userManagerMock
            .Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((User?)null);
        
        // creates a claimsPrincipal with a id 'User-1'.
        // this id is not in the database, so the controller will return a null user.
        ClaimsPrincipal principal = new(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "user-1")]));

        // passes through a null user to the controller
        // this will result in a 401 Unauthorized response
        AuctionController controller = CreateController(dbContext, userManagerMock.Object, principal); 

        CreateAuctionDto dto = new()
        {
            StartDate = DateTime.UtcNow,
            ClockLocationEnum = ClockLocationEnum.Naaldwijk,
            Products = []
        };

        // Act
        // returns a 401 Unauthorized response with the default message for not authorized
        ActionResult<Auction> actionResult = await controller.CreateAuction(dto);

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status401Unauthorized, json.StatusCode);
        Assert.Equal("Not authorized", GetJsonMessage(json));

        int auctionCount = await dbContext.Auctions.CountAsync();
        Assert.Equal(0, auctionCount);
    }

    [Fact]
    public async Task CreateAuction_WhenProductAlreadyBelongsToAuction_Returns400_AndMessage()
    {
        // Arrange
        await using ApplicationDbContext dbContext = CreateDbContext(); // creates an in-memory database
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock(); // creates a mock user manager

        User user = new() { Id = "user-1", UserName = "test" }; // creates a user with id 'user-1' and a user named 'test'

        // returns a user with id 'user-1'
        userManagerMock
            .Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        Product product = CreateValidProduct(id: 1, userId: user.Id, stock: 10);

        Auction existingAuction = new()
        {
            UserId = user.Id,
            ClockLocationEnum = ClockLocationEnum.Naaldwijk,
            StartDate = DateTime.UtcNow
        };

        // Act
        // Add the products and auction to the in-memory database
        dbContext.Products.Add(product); // creates first product
        dbContext.Auctions.Add(existingAuction);
        await dbContext.SaveChangesAsync();

        // Arrange
        AuctionProducts existingLink = new()
        {
            AuctionId = existingAuction.Id,
            ProductId = product.Id,
            ServeOrder = 1,
            AuctionStock = product.Stock
        };

        // Act
        // Add the AuctionProducts to the in-memory database
        dbContext.AuctionProducts.Add(existingLink);
        await dbContext.SaveChangesAsync();

        // Arrange
        // creates a claimsPrincipal with a id 'User-1'.
        // this id is in the database, so the controller will return a user with id 'user-1'.
        ClaimsPrincipal principal = new(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.Id)]));
        AuctionController controller = CreateController(dbContext, userManagerMock.Object, principal);

        CreateAuctionDto dto = new()
        {
            StartDate = DateTime.UtcNow,
            ClockLocationEnum = ClockLocationEnum.Naaldwijk,
            Products = [product]
        };

        // Act
        ActionResult<Auction> actionResult = await controller.CreateAuction(dto); // creates another auction with the same products which should return error

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, json.StatusCode);
        Assert.Equal("Product already belongs to an existing auction.", GetJsonMessage(json));
        Assert.Equal(1, await dbContext.Auctions.CountAsync());
    }

    [Fact]
    public async Task CreateAuction_WhenValid_Returns201_AndCreatesAuctionAndAuctionProducts()
    {
        // Arrange
        await using ApplicationDbContext dbContext = CreateDbContext(); // creates an in-memory database
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock(); // creates a mock user manager

        // returns a user with id 'user-1'
        User user = new() { Id = "user-1", UserName = "test" };

        // returns a user with id 'user-1'
        userManagerMock
            .Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        // Arrange
        // creates two products with some stock
        Product p1 = CreateValidProduct(id: 1, userId: user.Id, stock: 5);
        Product p2 = CreateValidProduct(id: 2, userId: user.Id, stock: 7);

        // Act
        dbContext.Products.AddRange(p1, p2);
        await dbContext.SaveChangesAsync();

        // Arrange
        // creates a claimsPrincipal with a id 'User-1'.
        // this id is in the database, so the controller will return a user with id 'user-1'.
        ClaimsPrincipal principal = new(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.Id)]));
        AuctionController controller = CreateController(dbContext, userManagerMock.Object, principal);

        DateTime startDate = DateTime.UtcNow;

        CreateAuctionDto dto = new()
        {
            StartDate = startDate,
            ClockLocationEnum = ClockLocationEnum.Aalsmeer,
            Products = [p1, p2]
        };

        // Act
        // returns a 201 Created response with the created auction and their products
        ActionResult<Auction> actionResult = await controller.CreateAuction(dto);

        // Assert
        // Asserts that the action result is a JsonResult with a 201 status code
        JsonResult json = Assert.IsType<JsonResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status201Created, json.StatusCode);

        // Asserts that the auction was created with the correct start date and clock location
        Auction? createdAuction = await dbContext.Auctions.FirstOrDefaultAsync(a => a.UserId == user.Id && a.StartDate == startDate);
        Assert.NotNull(createdAuction);
        Assert.Equal(ClockLocationEnum.Aalsmeer, createdAuction.ClockLocationEnum);
        
        // Asserts that the products were created with the correct serve order and stock
        List<AuctionProducts> links = await dbContext.AuctionProducts
            .Where(ap => ap.AuctionId == createdAuction.Id)
            .OrderBy(ap => ap.ServeOrder)
            .ToListAsync();

        Assert.Equal(2, links.Count);

        Assert.Equal(1, links[0].ServeOrder);
        Assert.Equal(p1.Id, links[0].ProductId);
        Assert.Equal(p1.Stock, links[0].AuctionStock);

        Assert.Equal(2, links[1].ServeOrder);
        Assert.Equal(p2.Id, links[1].ProductId);
        Assert.Equal(p2.Stock, links[1].AuctionStock);
    }
}
