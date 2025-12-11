using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.DTOs.Product;
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
public class AuctionController(IAuctionService auctionService, IProductService productService) : ControllerBase
{
    /// <summary>
    /// Get all auctions.
    /// </summary>
    /// <returns>A list of all auctions.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<Auction>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Auction>>> GetAuctions()
    {
        List<Auction> auctions = await auctionService.GetAuctions();
        return Ok(auctions);
    }

    /// <summary>
    /// Get an auction by ID.
    /// </summary>
    /// <param name="id">The auction ID.</param>
    /// <returns>The requested auction.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
    public async Task<ActionResult<Auction>> GetAuction(int id)
    {
        Auction auction = await auctionService.GetAuctionById(id);
        return Ok(auction);
    }

    /// <summary>
    /// Create a new auction.
    /// </summary>
    /// <param name="auctionData">The auction creation data.</param>
    /// <returns>The created auction.</returns>
    [HttpPost]
    [Authorize(Roles = "Auctioneer")]
    [ProducesResponseType(typeof(Auction), StatusCodes.Status201Created)]
    public async Task<ActionResult<Auction>> CreateAuction([FromBody] CreateAuctionDto auctionData)
    {
        Auction created = await auctionService.CreateAuction(auctionData, User);

        return CreatedAtAction(
            actionName: nameof(GetAuction),
            routeValues: new { id = created.Id, version = "2.0" },
            value: created
        );
    }

    /// <summary>
    /// Update an existing auction.
    /// </summary>
    /// <param name="id">The auction ID.</param>
    /// <param name="updatedAuction">The updated auction data.</param>
    /// <returns>The updated auction.</returns>
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(typeof(Auction), StatusCodes.Status200OK)]
    public async Task<ActionResult<Auction>> UpdateAuction(
        int id,
        [FromBody] UpdateAuctionDto updatedAuction)
    {
        Auction updated = await auctionService.UpdateAuction(id, updatedAuction);
        return Ok(updated);
    }

    /// <summary>
    /// Get products by auction ID.
    /// </summary>
    /// <param name="auctionId">The auction ID.</param>
    /// <returns>A list of products belonging to the auction.</returns>
    [HttpGet("products/{auctionId:int}")]
    [ProducesResponseType(typeof(List<ProductResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByAuctionId(int auctionId)
    {
        List<Product> products = await auctionService.GetProductsByAuctionId(auctionId);
        List<ProductResponse> productResponses = products
            .Select(productService.CreateProductResponse)
            .ToList();
        
        return Ok(productResponses);
    }
}
