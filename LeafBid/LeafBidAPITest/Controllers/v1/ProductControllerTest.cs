using LeafBidAPI.Controllers.v1;
using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LeafBidAPITest.Controllers.v1;

public class ProductControllerTest
{
    private static ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        var dbContext = new ApplicationDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    private static ProductController CreateController(ApplicationDbContext dbContext)
    {
        ProductController controller = new(dbContext);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        return controller;
    }

    private static ProductController CreateControllerWithUser(ApplicationDbContext dbContext, ClaimsPrincipal user)
    {
        ProductController controller = new(dbContext);
        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
        return controller;
    }

    private static ClaimsPrincipal CreateUser(string userId, string role)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, role)
        }, "mock");

        return new ClaimsPrincipal(identity);
    }

    private static Product CreateValidProduct(int id, string userId, int stock)
    {
        return new Product
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
    }

    // --- Tests ---

    // test if products get returned
    [Fact]
    public async Task GetProducts_WhenProductsExist_ReturnsList()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.Add(CreateValidProduct(1, "user-1", 5));
        dbContext.Products.Add(CreateValidProduct(2, "user-2", 10));
        await dbContext.SaveChangesAsync();
        var controller = CreateController(dbContext);

        // Act
        var result = await controller.GetProducts();

        // Assert
        var products = Assert.IsType<List<Product>>(result.Value);
        Assert.Equal(2, products.Count);
    }

    // test if database is empty

    [Fact]
    public async Task GetProducts_WhenProductsNotExist_ReturnsEmpryList()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var controller = CreateController(dbContext);
        
        // Act
        var result = await controller.GetProducts();
        
        // Assert
        var  products = Assert.IsType<List<Product>>(result.Value);
        Assert.Empty(products);
    }
    
    // test when available product exists

    [Fact]
    public async Task GetAvailableProducts_WhenStockPositive_ReturnsList()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.Add(CreateValidProduct(1, "user-1", 5));
        await dbContext.SaveChangesAsync();
        var controller = CreateController(dbContext);
        
        // Act
        var result = await controller.GetAvailableProducts();
        
        // Assert
        var products = Assert.IsType<List<Product>>(result.Value);
        Assert.Single(products);
    }
    
    // test when no available products are present

    [Fact]
    public async Task GetAvailableProducts_WhenStockNegative_ReturnsList()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.Add(CreateValidProduct(2, "user-1", 5));
        await dbContext.SaveChangesAsync();
        var controller = CreateController(dbContext);
        
        // Act
        var result = await controller.GetAvailableProducts();
        
        // Assert
        var products = Assert.IsType<List<Product>>(result.Value);
        Assert.Empty(products);
    }
    
    // test when product by id is found
    
    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsProduct()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = CreateValidProduct(1, "user-1", 5);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        var controller = CreateController(dbContext);
        
        // Act
        var result = await controller.GetProduct(1);
        
        // Assert
        var found = Assert.IsType<Product>(result.Value);
        Assert.Equal(product, found);
    }
    
    // test when id is wrong

    [Fact]
    public async Task GetProductById_WhenProductNotExists_Returns404()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        dbContext.Products.Add(CreateValidProduct(1, "user-1", 5));
        await dbContext.SaveChangesAsync();
        var controller = CreateController(dbContext);

        // Act
        var result = await controller.GetProduct(6767);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    
    // test when there are no products at all

    [Fact]
    public async Task GetProductById_WhenNoProducts_Returns404()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var controller = CreateController(dbContext);
        
        // Act
        var result = await controller.GetProduct(1);
        
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
    
    // test when product is added

    [Fact]
    public async Task CreateProduct_WhenValid_Return202()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);

        var dto = new CreateProductDto
        {
            Name = "Rose",
            Description = "Fresh rose",
            MinPrice = 1.5m,
            Weight = 0.2,
            Species = "Rose",
            Region = "NL",
            Stock = 10,
            HarvestedAt = DateTime.UtcNow,
            UserId = "user-1",
            Picture = ""
        };
        
        // Act
        var result = await controller.CreateProduct(dto);
        
        // Assert
        var json = Assert.IsType<JsonResult>(result.Result);
        Assert.Equal(StatusCodes.Status201Created, json.StatusCode);
        Assert.Equal(1, await dbContext.Products.CountAsync());
    }
    
    
    // test when harvested data is in the future
    
    [Fact]
    public async Task CreateProduct_WhenHarvestedInFuture_Returns400()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);
        
        var dto = new CreateProductDto
        {
            Name = "Rose",
            Description = "Fresh rose",
            MinPrice = 1.5m,
            Weight = 0.2,
            Species = "Rose",
            Region = "NL",
            Stock = 10,
            HarvestedAt = DateTime.UtcNow.AddDays(-10), // past date
            UserId = "user-1",
            Picture = ""
        };
        
        // Act
        var  result = await controller.CreateProduct(dto);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
    
    // test when role is not correct

    [Fact]
    public async Task CreateProduct_WhenRoleNotProvider_Returns403()
    {
        // Arrange 
        await using var dbContext = CreateDbContext();
        var user = CreateUser("user-1", "buyer");
        var controller = CreateControllerWithUser(dbContext,  user);
        
        var dto = new CreateProductDto
        {
            Name = "Rose",
            Description = "Fresh rose",
            MinPrice = 1.5m,
            Weight = 0.2,
            Species = "Rose",
            Region = "NL",
            Stock = 10,
            HarvestedAt = DateTime.UtcNow,
            UserId = "user-1",
            Picture = ""
        };
        
        // Act
        var result = await controller.CreateProduct(dto);
        
        // Assert
        JsonResult json = Assert.IsType<JsonResult>(result.Result);
        Assert.Equal(StatusCodes.Status403Forbidden, json.StatusCode);
    }
    
    // test when product is updated
    
    [Fact]
    public async Task UpdateProduct_WhenValid_Returns200()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = CreateValidProduct(1, "user-1", 5);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);
        
        var dto = new UpdateProductDto
        {
            Name = "Updated",
            Description = "Updated description",
            MinPrice = 2.0m,
            Weight = 0.3,
            Species = "Rose",
            Region = "NL",
            Stock = 20,
            HarvestedAt = DateTime.UtcNow,
            Picture = ""
        };
        
        // Act
        var result = await controller.UpdateProduct(product.Id, dto);
        
        // Assert
        var json = Assert.IsType<JsonResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, json.StatusCode);
    }
    
    // test when product is not found

    [Fact]
    public async Task UpdateProduct_WhenNotFound_Returns404()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);

        var dto = new UpdateProductDto
        {
            Name = "Updated product",
            Description = "Updated description",
            MinPrice = 2.0m,
            Weight = 0.3,
            Species = "Rose",
            Region = "NL",
            Stock = 20,
            HarvestedAt = DateTime.UtcNow,
            Picture = ""
        };
        
        // Act
        var result = await controller.UpdateProduct(6767, dto);
        
        // Assert
        JsonResult json = Assert.IsType<JsonResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, json.StatusCode);
    }
    
    // test when HarvestedAt is in the future
    
    [Fact]
    public async Task UpdateProduct_WhenHarvestedAtFuture_Returns400()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = CreateValidProduct(1, "user-1", 5);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);
        
        var dto = new UpdateProductDto
        {
            Name = "Updated product",
            Description = "Updated description",
            MinPrice = 2.0m,
            Weight = 0.3,
            Species = "Rose",
            Region = "NL",
            Stock = 20,
            HarvestedAt = DateTime.UtcNow.AddDays(10),
            Picture = ""
        };
        
        // Act
        var result = await controller.UpdateProduct(product.Id, dto);
        
        // Assert
        JsonResult json = Assert.IsType<JsonResult>(result.Result);
        Assert.Equal(StatusCodes.Status400BadRequest, json.StatusCode);
        
    }
    
    // test when role is not correct

    [Fact]
    public async Task UpdateProduct_WhenRoleNotProvider_Returns403()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = CreateValidProduct(1, "user-1", 5);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        
        var user = CreateUser("user-1", "Buyer");
        var controller = CreateControllerWithUser(dbContext, user);
        
        var dto = new UpdateProductDto
        {
            Name = "Updated product",
            Description = "Updated description",
            MinPrice = 2.0m,
            Weight = 0.3,
            Species = "Rose",
            Region = "NL",
            Stock = 20,
            HarvestedAt = DateTime.UtcNow,
            Picture = ""
        };
        
        // Act
        var result = await controller.UpdateProduct(product.Id, dto);
        
        // Assert
        JsonResult json = Assert.IsType<JsonResult>(result.Result);
        Assert.Equal(StatusCodes.Status403Forbidden, json.StatusCode);
    }
    
    // test when product is deleted
    
    [Fact]
    public async Task DeleteProduct_WhenValid_RemovesProduct()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = CreateValidProduct(1, "user-1", 5);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);
        
        // Act
        var result = await controller.DeleteProduct(product.Id);
        
        // Assert
        Assert.IsType<OkResult>(result);
        Assert.Equal(0, await dbContext.Products.CountAsync());
    }

    // test when product not found

    [Fact]
    public async Task DeleteProduct_WhenNotFound_Returns404()
    {
        // Arange
        await using var dbContext = CreateDbContext();
        var user = CreateUser("user-1", "Provider");
        var controller = CreateControllerWithUser(dbContext, user);
        
        // Act
        var result = await controller.DeleteProduct(67677);
        
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    // test when role is not provider

    [Fact]
    public async Task DeleteProduct_WhenRoleNotProvider_Returns403()
    {
        // Arrange
        await using var dbContext = CreateDbContext();
        var product = CreateValidProduct(1, "user-1", 5);
        dbContext.Products.Add(product);
        await dbContext.SaveChangesAsync();
        
        var user = CreateUser("user-1", "Buyer");
        var controller = CreateControllerWithUser(dbContext, user);
        
        // Act
        var result = await controller.DeleteProduct(product.Id);
        
        // Assert
        Assert.IsType<ForbidResult>(result);
    }



}
