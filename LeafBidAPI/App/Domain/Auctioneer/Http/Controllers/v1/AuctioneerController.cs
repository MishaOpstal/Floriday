using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auctioneer.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctioneerController(ApplicationDbContext context) : BaseController(context)
{
    
    /// <summary>
    /// Get all auctioneers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.Auctioneer>>> GetAuctioneers()
    {
        return await Context.Auctioneers.ToListAsync();
    }

    /// <summary>
    /// Get auctioneer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Auctioneer>> GetAuctioneer(int id)
    {
        var auctioneer = await Context.Auctioneers.FindAsync(id);
        if (auctioneer == null)
        {
            return NotFound();
        }

        return auctioneer;
    }

    /// <summary>
    /// Create a new auctioneer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.Auctioneer>> CreateAuctioneer(Models.Auctioneer auctioneer)
    {
        Context.Auctioneers.Add(auctioneer);
        await Context.SaveChangesAsync();

        return new JsonResult(auctioneer) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auctioneer
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuctioneer(int id, Models.Auctioneer updatedAuctioneer)
    {
        var auctioneer = await Context.Auctioneers.FindAsync(id);
        if (auctioneer == null)
        {
            return NotFound();
        }

        auctioneer.User = updatedAuctioneer.User;
        await Context.SaveChangesAsync();
        return new JsonResult(auctioneer);
    }
    
    /// <summary>
    /// Delete an auctioneer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuctioneer(int id)
    {
        var auctioneer = await Context.Auctioneers.FindAsync(id);
        if (auctioneer == null)
        {
            return NotFound();
        }

        Context.Auctioneers.Remove(auctioneer);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}