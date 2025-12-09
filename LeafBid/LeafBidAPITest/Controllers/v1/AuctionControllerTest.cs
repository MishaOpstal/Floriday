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
        ClaimsPrincipal? user = null)
    {
        AuctionController controller = new(dbContext, userManager);

        DefaultHttpContext httpContext = new()
        {
            User = user ?? new ClaimsPrincipal(new ClaimsIdentity())
        };

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

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

        userManagerMock
            .Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((User?)null);

        ClaimsPrincipal principal = new(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "user-1")]));

        AuctionController controller = CreateController(dbContext, userManagerMock.Object, principal);

        CreateAuctionDto dto = new()
        {
            StartDate = DateTime.UtcNow,
            ClockLocationEnum = ClockLocationEnum.Naaldwijk,
            Products = []
        };

        // Act
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
        await using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();

        User user = new() { Id = "user-1", UserName = "test" };

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
        dbContext.Products.Add(product);
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
        dbContext.AuctionProducts.Add(existingLink);
        await dbContext.SaveChangesAsync();

        // Arrange
        ClaimsPrincipal principal = new(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, user.Id)]));
        AuctionController controller = CreateController(dbContext, userManagerMock.Object, principal);

        CreateAuctionDto dto = new()
        {
            StartDate = DateTime.UtcNow,
            ClockLocationEnum = ClockLocationEnum.Naaldwijk,
            Products = [product]
        };

        // Act
        ActionResult<Auction> actionResult = await controller.CreateAuction(dto);

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
        await using ApplicationDbContext dbContext = CreateDbContext();
        Mock<UserManager<User>> userManagerMock = CreateUserManagerMock();

        User user = new() { Id = "user-1", UserName = "test" };

        userManagerMock
            .Setup(m => m.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(user);

        Product p1 = CreateValidProduct(id: 1, userId: user.Id, stock: 5);
        Product p2 = CreateValidProduct(id: 2, userId: user.Id, stock: 7);

        // Act
        dbContext.Products.AddRange(p1, p2);
        await dbContext.SaveChangesAsync();

        // Arrange
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
        ActionResult<Auction> actionResult = await controller.CreateAuction(dto);

        // Assert
        JsonResult json = Assert.IsType<JsonResult>(actionResult.Result);
        Assert.Equal(StatusCodes.Status201Created, json.StatusCode);

        Auction? createdAuction = await dbContext.Auctions.FirstOrDefaultAsync(a => a.UserId == user.Id && a.StartDate == startDate);
        Assert.NotNull(createdAuction);
        Assert.Equal(ClockLocationEnum.Aalsmeer, createdAuction.ClockLocationEnum);

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
