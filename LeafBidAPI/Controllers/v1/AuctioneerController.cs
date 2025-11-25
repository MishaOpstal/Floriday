using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Identity.Bearer")]
public class AuctioneerController(ApplicationDbContext context) : BaseController(context)
{
    
    /// <summary>
    /// Get all auctioneers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Auctioneer>>> GetAuctioneers()
    {
        return await Context.Auctioneers.ToListAsync();
    }

    /// <summary>
    /// Get auctioneer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Auctioneer>> GetAuctioneer(int id)
    {
        var auctioneer = await Context.Auctioneers.Where(a => a.Id == id).FirstOrDefaultAsync();
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
    public async Task<ActionResult<Auctioneer>> CreateAuctioneer(Auctioneer auctioneer)
    {
        Context.Auctioneers.Add(auctioneer);
        await Context.SaveChangesAsync();

        return new JsonResult(auctioneer) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auctioneer
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuctioneer(int id, Auctioneer updatedAuctioneer)
    {
        var auctioneer = await GetAuctioneer(id);
        if (auctioneer.Value == null)
        {
            return NotFound();
        }

        auctioneer.Value.User = updatedAuctioneer.User;
        await Context.SaveChangesAsync();
        return new JsonResult(auctioneer.Value);
    }
    
    /// <summary>
    /// Delete an auctioneer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuctioneer(int id)
    {
        var auctioneer = await GetAuctioneer(id);
        if (auctioneer.Value == null)
        {
            return NotFound();
        }

        Context.Auctioneers.Remove(auctioneer.Value);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}