using LeafBidAPI.Data;
using LeafBidAPI.Domain.AuctionSale.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleController(ApplicationDbContext context) : BaseController(context)
{
    
    /// <summary>
    /// Get all auction sales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSales>>> GetAuctionSales()
    {
        return await Context.AuctionSales.ToListAsync();
    }

    /// <summary>
    /// Get auction sale by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSales>> GetAuctionSale(int id)
    {
        var auctionSale = await Context.AuctionSales.FindAsync(id);
        if (auctionSale == null)
        {
            return NotFound();
        }

        return auctionSale;
    }
    
    /// <summary>
    /// Create a new auction sale
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSales>> CreateAuctionSale(AuctionSales auctionSale)
    {
        Context.AuctionSales.Add(auctionSale);
        await Context.SaveChangesAsync();

        return new JsonResult(auctionSale) { StatusCode = 201 };
    }
}