using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    /// <summary>
    /// Get all auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Auction>>> GetAuctions()
    {
        return await DbContext.Auctions.ToListAsync();
    }

    /// <summary>
    /// Get auction by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Auction>> GetAuction(int id)
    {
        var auction = await DbContext.Auctions.FindAsync(id);
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
    public async Task<ActionResult<Auction>> CreateAuction(Auction auction)
    {
        DbContext.Auctions.Add(auction);
        await DbContext.SaveChangesAsync();

        return new JsonResult(auction) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuction(int id, Auction updatedAuction)
    {
        var auction = await DbContext.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }

        auction.Description = updatedAuction.Description;
        auction.StartDate = updatedAuction.StartDate;
        auction.Amount = updatedAuction.Amount;
        auction.MinimumPrice = updatedAuction.MinimumPrice;
        auction.ClockLocationEnum = updatedAuction.ClockLocationEnum;
        auction.Auctioneer = updatedAuction.Auctioneer;
        await DbContext.SaveChangesAsync();
        return new JsonResult(auction);
    }
}