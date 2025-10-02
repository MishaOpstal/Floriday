using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuctionSalesProductController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<AuctionSalesProducts>>> GetAuctionSalesProducts()
    {
        return await DbContext.AuctionSalesProducts.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> GetAuctionSalesProduct(int id)
    {
        var auctionSaleProduct = await DbContext.AuctionSalesProducts.FindAsync(id);
        if (auctionSaleProduct == null)
            return NotFound();

        return auctionSaleProduct;
    }

    [HttpPost]
    public async Task<ActionResult<AuctionSalesProducts>> CreateAuctionSalesProducts(AuctionSalesProducts auctionSalesProduct)
    {
        DbContext.AuctionSalesProducts.Add(auctionSalesProduct);
        await DbContext.SaveChangesAsync();

        return new JsonResult(auctionSalesProduct) { StatusCode = 201 };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> UpdateAuctionSalesProducts(
        int id, AuctionSalesProducts auctionSalesProduct)
    {
        var AuctionSalesProducts = await DbContext.AuctionSalesProducts.FindAsync(id);
        if (AuctionSalesProducts == null)
            return NotFound();

        AuctionSalesProducts.AuctionSale = auctionSalesProduct.AuctionSale;
        AuctionSalesProducts.Product = auctionSalesProduct.Product;
        AuctionSalesProducts.Quantity = auctionSalesProduct.Quantity;
        AuctionSalesProducts.Price = auctionSalesProduct.Price;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(AuctionSalesProducts);
    }
}