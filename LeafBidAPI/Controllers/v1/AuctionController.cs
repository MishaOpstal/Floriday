using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
// [AllowAnonymous]
public class AuctionController(ApplicationDbContext context, UserManager<User> userManager) : BaseController(context)
{
    /// <summary>
    /// Get all auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Auction>>> GetAuctions()
    {
        return await Context.Auctions.ToListAsync();
    }

    /// <summary>
    /// Get auction by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Auction>> GetAuction(int id)
    {
        Auction? auction = await Context.Auctions.Where(a => a.Id == id).FirstOrDefaultAsync();
        if (auction == null)
        {
            return NotFound();
        }

        return auction;
    }

    /// <summary>
    /// Create a new auction
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Auctioneer")]
    public async Task<ActionResult<Auction>> CreateAuction(CreateAuctionDto auctionData)
    {
        User? currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized();
        }
        
        foreach (Product product in auctionData.Products)
        {
            // Check the AuctionProduct model
            AuctionProducts? auctionProducts = await Context.AuctionProducts.Where(a => a.ProductId == product.Id).FirstOrDefaultAsync();
            if (auctionProducts != null)
            {
                return BadRequest("Product already belongs to an existing auction.");
            }
        }

        Auction auction = new()
        {
            UserId = currentUser.Id,
            ClockLocationEnum = auctionData.ClockLocationEnum,
            StartDate = auctionData.StartDate
        };
        
        Context.Auctions.Add(auction);
        await Context.SaveChangesAsync();
        
        // Add the Products to the auction
        int counter = 1;
        foreach (Product product in auctionData.Products)
        {
            AuctionProducts auctionProduct = new()
            {
                AuctionId = auction.Id,
                ProductId = product.Id,
                ServeOrder = counter++, // Start at 1, then 2, etc.
                AuctionStock = product.Stock
            };
            
            Context.AuctionProducts.Add(auctionProduct);
        }
        
        // Update the products in db
        Context.Products.UpdateRange(auctionData.Products);
        await Context.SaveChangesAsync();

        return new JsonResult(auction) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<Auction>> UpdateAuction(int id, [FromBody] UpdateAuctionDto updatedAuction)
    {
        Auction? auction = await Context.Auctions.Where(a => a.Id == id).FirstOrDefaultAsync();

        if (auction == null)
        {
            return NotFound();
        }

        auction.StartDate = updatedAuction.StartTime;
        auction.ClockLocationEnum = updatedAuction.ClockLocationEnum;
        
        await Context.SaveChangesAsync();
        return new JsonResult(auction);
    }

    /// <summary>
    /// get products by AuctionId
    /// </summary>
    [HttpGet("products/{auctionId:int}")]
    public async Task<ActionResult<List<Product>>> GetProductsByAuctionId(int auctionId)
    {
        List<Product?> products = await Context.AuctionProducts
            .Where(ap => ap.AuctionId == auctionId)
            .OrderBy(ap => ap.ServeOrder)
            .Select(ap => ap.Product)
            .ToListAsync();
        
        if (products.Count == 0)
        {
            return NotFound();
        }

        ProductController productController = new(Context);
        List<ProductResponse> productResponse = products.OfType<Product>()
            .Select(product => productController.CreateProductResponse(product))
            .ToList();

        return new JsonResult(productResponse) { StatusCode = 200 };
    }
}