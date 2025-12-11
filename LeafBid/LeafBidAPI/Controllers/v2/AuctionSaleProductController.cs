using LeafBidAPI.DTOs.AuctionSaleProduct;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
[Authorize]
// [AllowAnonymous]
public class AuctionSaleProductController(IAuctionSaleProductService auctionSaleProductService) : ControllerBase
{
    /// <summary>
    /// Get all auction sale products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSalesProducts>>> GetAuctionSaleProducts()
    {
        return await auctionSaleProductService.GetAuctionSaleProducts();
    }

    /// <summary>
    /// Get an auction sale product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> GetAuctionSaleProduct(int id)
    {
        return await auctionSaleProductService.GetAuctionSaleProductById(id);
    }

    /// <summary>
    /// Create a new auction sale product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSalesProducts>> CreateAuctionSaleProducts([FromBody] CreateAuctionSaleProductDto auctionSaleProductData)
    {
        return await auctionSaleProductService.CreateAuctionSaleProduct(auctionSaleProductData);
    }

    /// <summary>
    /// Update an existing auction sale product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSalesProducts>> UpdateAuctionSaleProducts(
        int id, [FromBody]UpdateAuctionSaleProductDto updatedAuctionSaleProduct)
    {
        return await auctionSaleProductService.UpdateAuctionSaleProduct(id, updatedAuctionSaleProduct);
    }
}