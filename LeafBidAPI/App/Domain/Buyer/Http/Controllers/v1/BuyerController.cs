using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.Buyer.Data;
using LeafBidAPI.App.Domain.Buyer.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.Buyer.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Buyer.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BuyerController(
    ApplicationDbContext context,
    BuyerRepository buyerRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all buyers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<BuyerResource>>> GetBuyers()
    {
        var buyers = await Context.Buyers
            .AsNoTracking()
            .ProjectTo<BuyerResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(buyers) { StatusCode = 200 };
    }

    /// <summary>
    /// Get a buyer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<BuyerResource>> GetBuyer(int id)
    {
        var result = await buyerRepository.GetBuyerAsync(new GetBuyerData(id));

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<BuyerResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new buyer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<BuyerResource>> CreateBuyer([FromBody] CreateBuyerRequest request)
    {
        var result = await buyerRepository.CreateBuyerAsync(
            new CreateBuyerData(request.UserId, request.CompanyName)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<BuyerResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing buyer
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BuyerResource>> UpdateBuyer(int id, [FromBody] UpdateBuyerRequest request)
    {
        var result = await buyerRepository.UpdateBuyerAsync(
            new UpdateBuyerData(id, request.UserId, request.CompanyName)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<BuyerResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Delete a buyer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBuyer(int id)
    {
        var result = await buyerRepository.DeleteBuyerAsync(new DeleteBuyerData(id));

        return result.IsFailed ? BadRequest(result.Errors) : new OkResult();
    }
}

public record CreateBuyerRequest(int UserId, string CompanyName);
public record UpdateBuyerRequest(int UserId, string CompanyName);