using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    
    /// <summary>
    /// Get all auction sales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSales>>> GetAuctionSales()
    {
        return await DbContext.AuctionSales.ToListAsync();
    }

    /// <summary>
    /// Get auction sale by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSales>> GetAuctionSale(int id)
    {
        var auctionSale = await DbContext.AuctionSales.FindAsync(id);
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
        DbContext.AuctionSales.Add(auctionSale);
        await DbContext.SaveChangesAsync();

        return new JsonResult(auctionSale) { StatusCode = 201 };
    }
}