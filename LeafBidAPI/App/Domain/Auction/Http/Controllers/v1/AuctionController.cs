using LeafBidAPI.App.Domain.Auction.Data;
using LeafBidAPI.App.Domain.Auction.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
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
    public Task<ActionResult<Models.Auction>> CreateAuction(Models.Auction auctionData)
    {
        var auction = auctionRepository.CreateAuctionAsync(
            new CreateAuctionData(
                auctionData.Description,
                auctionData.StartDate,
                auctionData.Amount,
                auctionData.MinimumPrice,
                auctionData.ClockLocationEnum,
                auctionData.AuctioneerId
            )
        );

        return Task.FromResult<ActionResult<Models.Auction>>(new JsonResult(auction) { StatusCode = 201 });
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuction(int id, Models.Auction auctionData)
    {
        var auction = await auctionRepository.UpdateAuctionAsync(
            new UpdateAuctionData(
                id,
                auctionData.Description,
                auctionData.StartDate,
                auctionData.Amount,
                auctionData.MinimumPrice,
                auctionData.ClockLocationEnum
            )
        );
        
        return auction.IsFailed ? NotFound() : new JsonResult(auction.Value) { StatusCode = 200 };
    }
}