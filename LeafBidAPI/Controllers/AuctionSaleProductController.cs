using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuctionSaleProductController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<AuctionSalesProducts>>> GetAuctionSaleProducts()
    {
        return await DbContext.AuctionSalesProducts.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> GetAuctionSaleProduct(int id)
    {
        var auctionSaleProduct = await DbContext.AuctionSalesProducts.FindAsync(id);
        if (auctionSaleProduct == null)
        {
            return NotFound();
        }

        return auctionSaleProduct;
    }

    [HttpPost]
    public async Task<ActionResult<AuctionSalesProducts>> CreateAuctionSaleProducts(AuctionSalesProducts auctionSaleProduct)
    {
        DbContext.AuctionSalesProducts.Add(auctionSaleProduct);
        await DbContext.SaveChangesAsync();

        return new JsonResult(auctionSaleProduct) { StatusCode = 201 };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> UpdateAuctionSaleProducts(
        int id, AuctionSalesProducts updatedAuctionSaleProduct)
    {
        var auctionSaleProduct = await DbContext.AuctionSalesProducts.FindAsync(id);
        if (auctionSaleProduct == null)
        {
            return NotFound();
        }

        auctionSaleProduct.AuctionSale = updatedAuctionSaleProduct.AuctionSale;
        auctionSaleProduct.Product = updatedAuctionSaleProduct.Product;
        auctionSaleProduct.Quantity = updatedAuctionSaleProduct.Quantity;
        auctionSaleProduct.Price = updatedAuctionSaleProduct.Price;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(auctionSaleProduct);
    }
}