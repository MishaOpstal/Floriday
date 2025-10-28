using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auction.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionController(ApplicationDbContext context) : BaseController(context)
{
    /// <summary>
    /// Get all auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Entities.Auction>>> GetAuctions()
    {
        return await Context.Auctions.ToListAsync();
    }

    /// <summary>
    /// Get auction by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Entities.Auction>> GetAuction(int id)
    {
        var auction = await Context.Auctions.FindAsync(id);
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
    public async Task<ActionResult<Entities.Auction>> CreateAuction(Entities.Auction auction)
    {
        Context.Auctions.Add(auction);
        await Context.SaveChangesAsync();

        return new JsonResult(auction) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuction(int id, Entities.Auction updatedAuction)
    {
        var auction = await Context.Auctions.FindAsync(id);
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
        await Context.SaveChangesAsync();
        return new JsonResult(auction);
    }
}