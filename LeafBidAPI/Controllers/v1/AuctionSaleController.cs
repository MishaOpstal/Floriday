using LeafBidAPI.Data;
using LeafBidAPI.DTOs.AuctionSale;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
// [AllowAnonymous]
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
        var auctionSale = await Context.AuctionSales.Where(sale => sale.Id == id).FirstOrDefaultAsync();
        if (auctionSale == null)
        {
            return NotFound();
        }

        return auctionSale;
    }
//   public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productData)
    /// <summary>
    /// Create a new auction sale
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSales>> CreateAuctionSale([FromBody] CreateAuctionSaleDto auctionSaleData)
    {
        AuctionSales auctionSale = new()
        {
            AuctionId = auctionSaleData.AuctionId,
            UserId = auctionSaleData.UserId,
            PaymentReference = auctionSaleData.PaymentReference,
            Date = auctionSaleData.Date
        };
        await Context.SaveChangesAsync();

        return new JsonResult(auctionSaleData) { StatusCode = 201 };
    }
}