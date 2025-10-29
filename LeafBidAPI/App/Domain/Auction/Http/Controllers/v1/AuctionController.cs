using LeafBidAPI.App.Domain.Auction.Data;
using LeafBidAPI.App.Domain.Auction.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using LeafBidAPI.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auction.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionController(ApplicationDbContext context, AuctionRepository auctionRepository) : BaseController(context)
{
    /// <summary>
    /// Get all auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.Auction>>> GetAuctions()
    {
        return await Context.Auctions.ToListAsync();
    }

    /// <summary>
    /// Get auction by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Auction>> GetAuction(int id)
    {
        var auction = await auctionRepository.GetAuctionAsync(
            new GetAuctionData(id)
        );
        
        return auction.IsFailed ? NotFound() : new JsonResult(auction.Value) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auction
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.Auction>> CreateAuction([FromBody] CreateAuctionRequest request)
    {
        var auction = await auctionRepository.CreateAuctionAsync(
            new CreateAuctionData(
                request.Description,
                request.StartDate,
                request.Amount,
                request.MinimumPrice,
                request.ClockLocationEnum,
                request.AuctioneerId
            )
        );
        
        return auction.IsFailed
            ? BadRequest(auction.Errors)
            : new JsonResult(auction.Value) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuction(int id, [FromBody] UpdateAuctionRequest request)
    {
        var auction = await auctionRepository.UpdateAuctionAsync(
            new UpdateAuctionData(
                id,
                request.Description,
                request.StartDate,
                request.Amount,
                request.MinimumPrice,
                request.ClockLocationEnum
            )
        );
        
        return auction.IsFailed ? NotFound() : new JsonResult(auction.Value) { StatusCode = 200 };
    }
}

public record CreateAuctionRequest(
    string Description,
    DateTime StartDate,
    int Amount,
    decimal MinimumPrice,
    ClockLocationEnum ClockLocationEnum,
    int AuctioneerId
);

public record UpdateAuctionRequest(
    string Description,
    DateTime StartDate,
    int Amount,
    decimal MinimumPrice,
    ClockLocationEnum ClockLocationEnum
);