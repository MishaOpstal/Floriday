using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuctionSaleController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<AuctionSales>>> GetAuctionSales()
    {
        return await DbContext.AuctionSales.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSales>> GetAuctionSale(int id)
    {
        var auctionSale = await DbContext.AuctionSales.FindAsync(id);
        if (auctionSale == null)
            return NotFound();

        return auctionSale;
    }

    [HttpPost]
    public async Task<ActionResult<AuctionSales>> CreateAuctionSale(AuctionSales auctionSale)
    {
        DbContext.AuctionSales.Add(auctionSale);
        await DbContext.SaveChangesAsync();

        return new JsonResult(auctionSale) { StatusCode = 201 };
    }
}