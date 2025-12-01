using LeafBidAPI.Data;
using LeafBidAPI.DTOs.AuctionSaleProduct;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
// [AllowAnonymous]
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
    public async Task<ActionResult<AuctionSalesProducts>> CreateAuctionSaleProducts([FromBody] CreateAuctionSaleProductDto auctionSaleProductData)
    {
        AuctionSalesProducts auctionSaleProduct = new()
        {
            AuctionSaleId = auctionSaleProductData.AuctionSaleId,
            ProductId = auctionSaleProductData.ProductId,
            Quantity = auctionSaleProductData.Quantity,
            Price = auctionSaleProductData.Price
        };
        await Context.SaveChangesAsync();

        return new JsonResult(auctionSaleProduct) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> UpdateAuctionSaleProducts(
        int id, [FromBody]UpdateAuctionSaleProductDto updatedAuctionSaleProduct)
    {
        ActionResult<AuctionSalesProducts> auctionSaleProduct = await GetAuctionSaleProduct(id);
        AuctionSalesProducts auctionSaleProducts = auctionSaleProduct.Value;
        
        if (auctionSaleProducts == null)
        {
            return NotFound();
        }

        auctionSaleProducts.Id = updatedAuctionSaleProduct.Id;
        auctionSaleProducts.AuctionSaleId = updatedAuctionSaleProduct.AuctionSaleId;
        auctionSaleProducts.ProductId = updatedAuctionSaleProduct.ProductId;
        auctionSaleProducts.Quantity = updatedAuctionSaleProduct.Quantity;
        auctionSaleProducts.Price = updatedAuctionSaleProduct.Price;
        

        await Context.SaveChangesAsync();
        return new JsonResult(auctionSaleProducts);
    }
    
    // /// <summary>
    // /// Update an existing auction
    // /// </summary>
    // [HttpPut("{id:int}")]
    // [Authorize]
    // public async Task<ActionResult<Auction>> UpdateAuction(int id, [FromBody] UpdateAuctionDto updatedAuction)
    // {
    //     ActionResult<Auction> auctionResult = await GetAuction(id);
    //     Auction auction = auctionResult.Value;
    //
    //     if (auction == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     auction.StartDate = updatedAuction.StartTime;
    //     auction.ClockLocationEnum = updatedAuction.ClockLocationEnum;
    //     auction.UserId = updatedAuction.UserId;
    //
    //     await Context.SaveChangesAsync();
    //     return new JsonResult(auction);
    // }
}