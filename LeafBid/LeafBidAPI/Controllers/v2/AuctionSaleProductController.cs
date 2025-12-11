using LeafBidAPI.DTOs.AuctionSaleProduct;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
// [Authorize]
[AllowAnonymous]
[Produces("application/json")]
public class AuctionSaleProductController(IAuctionSaleProductService auctionSaleProductService) : ControllerBase
{
    /// <summary>
    /// Get all auction sale products.
    /// </summary>
    /// <returns>A list of all auction sale products.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<AuctionSalesProducts>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuctionSalesProducts>>> GetAuctionSaleProducts()
    {
        List<AuctionSalesProducts> items = await auctionSaleProductService.GetAuctionSaleProducts();
        return Ok(items);
    }

    /// <summary>
    /// Get an auction sale product by ID.
    /// </summary>
    /// <param name="id">The auction sale product ID.</param>
    /// <returns>The requested auction sale product.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AuctionSalesProducts), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)] 
    public async Task<ActionResult<AuctionSalesProducts>> GetAuctionSaleProduct(int id)
    {
        try
        {
            AuctionSalesProducts item = await auctionSaleProductService.GetAuctionSaleProductById(id);
            return Ok(item);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }

    /// <summary>
    /// Create a new auction sale product.
    /// </summary>
    /// <param name="auctionSaleProductData">The auction sale product data.</param>
    /// <returns>The created auction sale product.</returns>
    [HttpPost]
    [Authorize(Roles = "Provider")]
    [ProducesResponseType(typeof(AuctionSalesProducts), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuctionSalesProducts>> CreateAuctionSaleProducts(
        [FromBody] CreateAuctionSaleProductDto auctionSaleProductData)
    {
        AuctionSalesProducts created = await auctionSaleProductService.CreateAuctionSaleProduct(auctionSaleProductData);

        return CreatedAtAction(
            actionName: nameof(GetAuctionSaleProduct),
            routeValues: new { id = created.Id, version = "2.0" },
            value: created
        );
    }

    /// <summary>
    /// Update an existing auction sale product.
    /// </summary>
    /// <param name="id">The auction sale product ID.</param>
    /// <param name="updatedAuctionSaleProduct">The updated auction sale product data.</param>
    /// <returns>The updated auction sale product.</returns>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Provider")]
    [ProducesResponseType(typeof(AuctionSalesProducts), StatusCodes.Status200OK)]
    public async Task<ActionResult<AuctionSalesProducts>> UpdateAuctionSaleProducts(
        int id,
        [FromBody] UpdateAuctionSaleProductDto updatedAuctionSaleProduct)
    {
        AuctionSalesProducts updated =
            await auctionSaleProductService.UpdateAuctionSaleProduct(id, updatedAuctionSaleProduct);

        return Ok(updated);
    }
}
