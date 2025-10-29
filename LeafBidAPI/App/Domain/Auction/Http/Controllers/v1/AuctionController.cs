using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.Auction.Data;
using LeafBidAPI.App.Domain.Auction.Enums;
using LeafBidAPI.App.Domain.Auction.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.Auction.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auction.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionController(
    ApplicationDbContext context,
    AuctionRepository auctionRepository,
    IMapper mapper) : BaseController(context)
{
    /// <summary>
    /// Get all auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionResource>>> GetAuctions()
    {
        // Project directly using AutoMapper
        var auctions = await Context.Auctions
            .AsNoTracking()
            .ProjectTo<AuctionResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(auctions) { StatusCode = 200 };
    }

    /// <summary>
    /// Get auction by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionResource>> GetAuction(int id)
    {
        var auctionResult = await auctionRepository.GetAuctionAsync(new GetAuctionData(id));

        if (auctionResult.IsFailed)
            return NotFound();

        // Map entity to resource using AutoMapper
        var resource = mapper.Map<AuctionResource>(auctionResult.Value);
        
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auction
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionResource>> CreateAuction([FromBody] CreateAuctionRequest request)
    {
        var auctionResult = await auctionRepository.CreateAuctionAsync(
            new CreateAuctionData(
                request.Description,
                request.StartDate,
                request.Amount,
                request.MinimumPrice,
                request.ClockLocationEnum,
                request.AuctioneerId
            )
        );

        if (auctionResult.IsFailed)
            return BadRequest(auctionResult.Errors);

        // Map entity to resource
        var resource = mapper.Map<AuctionResource>(auctionResult.Value);

        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionResource>> UpdateAuction(int id, [FromBody] UpdateAuctionRequest request)
    {
        var auctionResult = await auctionRepository.UpdateAuctionAsync(
            new UpdateAuctionData(
                id,
                request.Description,
                request.StartDate,
                request.Amount,
                request.MinimumPrice,
                request.ClockLocationEnum
            )
        );

        if (auctionResult.IsFailed)
            return BadRequest(auctionResult.Errors);

        // Map entity to resource
        var resource = mapper.Map<AuctionResource>(auctionResult.Value);

        return new JsonResult(resource) { StatusCode = 200 };
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