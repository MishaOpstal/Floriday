using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleProductController(ApplicationDbContext context) : BaseController(context)
{
    /// <summary>
    /// Get all auction sale products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.AuctionSaleProduct>>> GetAuctionSaleProducts()
    {
        return await Context.AuctionSaleProducts.ToListAsync();
    }

    /// <summary>
    /// Get an auction sale product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.AuctionSaleProduct>> GetAuctionSaleProduct(int id)
    {
        var auctionSaleProduct = await Context.AuctionSaleProducts.FindAsync(id);
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
    public async Task<ActionResult<Models.AuctionSaleProduct>> CreateAuctionSaleProducts(Models.AuctionSaleProduct auctionSaleProduct)
    {
        Context.AuctionSaleProducts.Add(auctionSaleProduct);
        await Context.SaveChangesAsync();

        return new JsonResult(auctionSaleProduct) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Models.AuctionSaleProduct>> UpdateAuctionSaleProducts(
        int id, Models.AuctionSaleProduct updatedAuctionSaleProduct)
    {
        var auctionSaleProduct = await Context.AuctionSaleProducts.FindAsync(id);
        if (auctionSaleProduct == null)
        {
            return NotFound();
        }

        auctionSaleProduct.AuctionSale = updatedAuctionSaleProduct.AuctionSale;
        auctionSaleProduct.Product = updatedAuctionSaleProduct.Product;
        auctionSaleProduct.Quantity = updatedAuctionSaleProduct.Quantity;
        auctionSaleProduct.Price = updatedAuctionSaleProduct.Price;
        
        await Context.SaveChangesAsync();
        return new JsonResult(auctionSaleProduct);
    }
}