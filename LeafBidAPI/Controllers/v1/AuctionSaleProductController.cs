using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleProductController(ApplicationDbContext context) : BaseController(context)
{
    /// <summary>
    /// Get all auction sale products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSalesProducts>>> GetAuctionSaleProducts()
    {
        return await Context.AuctionSalesProducts.ToListAsync();
    }

    /// <summary>
    /// Get an auction sale product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> GetAuctionSaleProduct(int id)
    {
        var auctionSaleProduct = await Context.AuctionSalesProducts.Where(asp => asp.Id == id).FirstOrDefaultAsync();
        if (auctionSaleProduct == null)
        {
            return NotFound();
        }

        return auctionSaleProduct;
    }

    /// <summary>
    /// Create a new auction sale product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSalesProducts>> CreateAuctionSaleProducts(AuctionSalesProducts auctionSaleProduct)
    {
        Context.AuctionSalesProducts.Add(auctionSaleProduct);
        await Context.SaveChangesAsync();

        return new JsonResult(auctionSaleProduct) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> UpdateAuctionSaleProducts(
        int id, AuctionSalesProducts updatedAuctionSaleProduct)
    {
        var auctionSaleProduct = await GetAuctionSaleProduct(id);
        if (auctionSaleProduct.Value == null)
        {
            return NotFound();
        }

        auctionSaleProduct.Value.AuctionSale = updatedAuctionSaleProduct.AuctionSale;
        auctionSaleProduct.Value.Product = updatedAuctionSaleProduct.Product;
        auctionSaleProduct.Value.Quantity = updatedAuctionSaleProduct.Quantity;
        auctionSaleProduct.Value.Price = updatedAuctionSaleProduct.Price;
        
        await Context.SaveChangesAsync();
        return new JsonResult(auctionSaleProduct.Value);
    }
}