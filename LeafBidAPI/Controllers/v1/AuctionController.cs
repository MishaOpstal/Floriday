using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Identity.Bearer")]
public class AuctionController(ApplicationDbContext context) : BaseController(context)
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
        var auction = await Context.Auctions.Where(a => a.Id == id).FirstOrDefaultAsync();
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
    [Authorize(AuthenticationSchemes = "Identity.Bearer", Roles = "Auctioneer")]
    public async Task<ActionResult<Auction>> CreateAuction(Auction auction)
    {
        Context.Auctions.Add(auction);
        await Context.SaveChangesAsync();

        return new JsonResult(auction) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(AuthenticationSchemes = "Identity.Bearer", Roles = "Auctioneer")]
    public async Task<ActionResult> UpdateAuction(int id, Auction updatedAuction)
    {
        var auction = await GetAuction(id);
        if (auction.Value == null)
        {
            return NotFound();
        }

        auction.Value.StartDate = updatedAuction.StartDate;
        auction.Value.ClockLocationEnum = updatedAuction.ClockLocationEnum;
        auction.Value.Auctioneer = updatedAuction.Auctioneer;
        await Context.SaveChangesAsync();
        return new JsonResult(auction.Value);
    }
}