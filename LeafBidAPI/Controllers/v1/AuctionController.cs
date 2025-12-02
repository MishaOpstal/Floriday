using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Auction;
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
            if (product.AuctionId != null)
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
        foreach (Product product in auctionData.Products)
        {
            product.AuctionId = auction.Id;
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
}