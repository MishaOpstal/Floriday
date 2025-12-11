using System.Security.Claims;
using LeafBidAPI.Controllers.v1;
using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LeafBidAPITest.Controllers.v1;

public class AuctionSaleControllerTest
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


        AuctionSaleController testcontroller = new(dbContext);
        
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
}



// /// <summary>
// /// Create a new auction sale
// /// </summary>
// [HttpPost]
// public async Task<ActionResult<AuctionSales>> CreateAuctionSale([FromBody] CreateAuctionSaleDto auctionSaleData)
// {
//     AuctionSales auctionSale = new()
//     {
//         AuctionId = auctionSaleData.AuctionId,
//         UserId = auctionSaleData.UserId,
//         PaymentReference = auctionSaleData.PaymentReference,
//         Date = auctionSaleData.Date
//     };
//         
//     // Create the auction sale
//     Context.AuctionSales.Add(auctionSale);
//     await Context.SaveChangesAsync();
//
//     return new JsonResult(auctionSale) { StatusCode = 201 };
// }
// }