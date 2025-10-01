using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuctionController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<Auction>>> GetAuctions()
    {
        return await DbContext.Auctions.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Auction>> GetAuction(int id)
    {
        var auction = await DbContext.Auctions.FindAsync(id);
        if (auction == null)
            return NotFound();

        return auction;
    }
}