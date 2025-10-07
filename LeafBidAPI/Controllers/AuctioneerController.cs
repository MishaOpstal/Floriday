using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuctioneerController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<Auctioneer>>> GetAuctioneers()
    {
        return await DbContext.Auctioneers.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Auctioneer>> GetAuctioneer(int id)
    {
        var auctioneer = await DbContext.Auctioneers.FindAsync(id);
        if (auctioneer == null)
        {
            return NotFound();
        }

        return auctioneer;
    }

    [HttpPost]
    public async Task<ActionResult<Auctioneer>> CreateAuctioneer(Auctioneer auctioneer)
    {
        DbContext.Auctioneers.Add(auctioneer);
        await DbContext.SaveChangesAsync();

        return new JsonResult(auctioneer) { StatusCode = 201 };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuctioneer(int id, Auctioneer updatedAuctioneer)
    {
        var auctioneer = await DbContext.Auctioneers.FindAsync(id);
        if (auctioneer == null)
        {
            return NotFound();
        }

        auctioneer.User = updatedAuctioneer.User;
        await DbContext.SaveChangesAsync();
        return new JsonResult(auctioneer);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAuctioneer(int id)
    {
        var auctioneer = await DbContext.Auctioneers.FindAsync(id);
        if (auctioneer == null)
        {
            return NotFound();
        }

        DbContext.Auctioneers.Remove(auctioneer);
        await DbContext.SaveChangesAsync();
        return new OkResult();
    }
}