using LeafBidAPI.DTOs.AuctionSale;
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
public class AuctionSaleController(IAuctionSaleService auctionSaleService) : ControllerBase
{
    /// <summary>
    /// Get all auction sales.
    /// </summary>
    /// <returns>A list of all auction sales.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<AuctionSales>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AuctionSales>>> GetAuctionSales()
    {
        List<AuctionSales> sales = await auctionSaleService.GetAuctionSales();
        return Ok(sales);
    }

    /// <summary>
    /// Get an auction sale by ID.
    /// </summary>
    /// <param name="id">The auction sale ID.</param>
    /// <returns>The requested auction sale.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AuctionSales), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AuctionSales>> GetAuctionSaleById(int id)
    {
        try
        {
            AuctionSales sale = await auctionSaleService.GetAuctionSaleById(id);
            return Ok(sale);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    /// <summary>
    /// Create a new auction sale.
    /// </summary>
    /// <param name="auctionSaleData">The auction sale data.</param>
    /// <returns>The created auction sale.</returns>
    [HttpPost]
    [Authorize(Roles = "Provider")]
    [ProducesResponseType(typeof(AuctionSales), StatusCodes.Status201Created)]
    public async Task<ActionResult<AuctionSales>> CreateAuctionSale(
        [FromBody] CreateAuctionSaleDto auctionSaleData)
    {
        AuctionSales created = await auctionSaleService.CreateAuctionSale(auctionSaleData);

        return CreatedAtAction(
            actionName: nameof(GetAuctionSaleById),
            routeValues: new { id = created.Id, version = "2.0" },
            value: created
        );
    }
}