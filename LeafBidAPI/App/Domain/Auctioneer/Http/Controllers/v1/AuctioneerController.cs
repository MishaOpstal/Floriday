using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.Auctioneer.Data;
using LeafBidAPI.App.Domain.Auctioneer.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.Auctioneer.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auctioneer.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctioneerController(
    ApplicationDbContext context,
    AuctioneerRepository auctioneerRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all auctioneers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctioneerResource>>> GetAuctioneers()
    {
        var auctioneers = await Context.Auctioneers
            .AsNoTracking()
            .ProjectTo<AuctioneerResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(auctioneers) { StatusCode = 200 };
    }

    /// <summary>
    /// Get auctioneer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctioneerResource>> GetAuctioneer(int id)
    {
        var result = await auctioneerRepository.GetAuctioneerAsync(new GetAuctioneerData(id));

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<AuctioneerResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auctioneer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctioneerResource>> CreateAuctioneer([FromBody] CreateAuctioneerRequest request)
    {
        var result = await auctioneerRepository.CreateAuctioneerAsync(
            new CreateAuctioneerData(request.UserId)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<AuctioneerResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Delete an auctioneer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuctioneer(int id)
    {
        var result = await auctioneerRepository.DeleteAuctioneerAsync(new DeleteAuctioneerData(id));

        return result.IsFailed ? NotFound() : new OkResult();
    }
}

public record CreateAuctioneerRequest(int UserId);