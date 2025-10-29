using LeafBidAPI.App.Domain.Auctioneer.Data;
using LeafBidAPI.App.Domain.Auctioneer.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auctioneer.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctioneerController(ApplicationDbContext context, AuctioneerRepository auctioneerRepository) : BaseController(context)
{
    
    /// <summary>
    /// Get all auctioneers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.Auctioneer>>> GetAuctioneers()
    {
        return await Context.Auctioneers.ToListAsync();
    }

    /// <summary>
    /// Get auctioneer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Auctioneer>> GetAuctioneer(int id)
    {
        var auctioneer = await auctioneerRepository.GetAuctioneerAsync(
            new GetAuctioneerData(id)
        );

        return auctioneer.IsFailed ? NotFound() : new JsonResult(auctioneer.Value) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auctioneer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.Auctioneer>> CreateAuctioneer(Models.Auctioneer auctioneer)
    {
        var createdAuctioneer = await auctioneerRepository.CreateAuctioneerAsync(
            new CreateAuctioneerData(
                auctioneer.UserId
            )
        );

        return createdAuctioneer.IsFailed
            ? BadRequest(createdAuctioneer.Errors)
            : new JsonResult(createdAuctioneer.Value) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Delete an auctioneer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuctioneer(int id)
    {
        var auctioneer = await auctioneerRepository.DeleteAuctioneerAsync(
            new DeleteAuctioneerData(id)
        );
        
        return auctioneer.IsFailed ? NotFound() : new OkResult();
    }
}