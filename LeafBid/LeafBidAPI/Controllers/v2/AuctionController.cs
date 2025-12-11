using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
// [Authorize]
[AllowAnonymous]
[ApiVersion("2.0")]
[Produces("application/json")]
public class AuctionController(IAuctionService auctionService) : ControllerBase
{
    /// <summary>
    /// Get all auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Auction>>> GetAuctions()
    {
        return await auctionService.GetAuctions();
    }

    /// <summary>
    /// Get auction by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Auction>> GetAuction(int id)
    {
        return await auctionService.GetAuctionById(id);
    }

    /// <summary>
    /// Create a new auction
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Auctioneer")]
    public async Task<ActionResult<Auction>> CreateAuction(CreateAuctionDto auctionData)
    {
        return await auctionService.CreateAuction(auctionData, User);
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<ActionResult<Auction>> UpdateAuction(int id, [FromBody] UpdateAuctionDto updatedAuction)
    {
        return await auctionService.UpdateAuction(id, updatedAuction);
    }

    /// <summary>
    /// get products by AuctionId
    /// </summary>
    [HttpGet("products/{auctionId:int}")]
    public async Task<ActionResult<List<ProductResponse>>> GetProductsByAuctionId(int auctionId)
    {
        return await auctionService.GetProductsByAuctionId(auctionId);
    }
}