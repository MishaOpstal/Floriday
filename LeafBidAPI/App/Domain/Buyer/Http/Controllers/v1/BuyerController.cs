using LeafBidAPI.App.Domain.Buyer.Data;
using LeafBidAPI.App.Domain.Buyer.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Buyer.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BuyerController(ApplicationDbContext context, BuyerRepository buyerRepository) : BaseController(context)
{
    
    /// <summary>
    /// Get all buyers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.Buyer>>> GetBuyers()
    {
        return await Context.Buyers.ToListAsync();
    }

    /// <summary>
    /// Get a buyer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Buyer>> GetBuyer(int id)
    {
        var buyer = await buyerRepository.GetBuyerAsync(
            new GetBuyerData(id)
        );
        
        return buyer.IsFailed ? NotFound() : new JsonResult(buyer.Value) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new buyer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.Buyer>> CreateBuyer([FromBody] CreateBuyerRequest request)
    {
        var buyer = await buyerRepository.CreateBuyerAsync(
            new CreateBuyerData(
                request.UserId,
                request.CompanyName
            )
        );
        
        return buyer.IsFailed ? BadRequest(buyer.Errors) : new JsonResult(buyer.Value) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing buyer
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBuyer(int id, [FromBody] UpdateBuyerRequest request)
    {
        var buyer = await buyerRepository.UpdateBuyerAsync(
            new UpdateBuyerData(
                id,
                request.UserId,
                request.CompanyName
            )
        );
        
        return buyer.IsFailed ? BadRequest(buyer.Errors) : new JsonResult(buyer.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Delete a buyer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBuyer(int id)
    {
        var result = await buyerRepository.DeleteBuyerAsync(
            new DeleteBuyerData(id)
        );
        
        return result.IsFailed ? BadRequest(result.Errors) : new OkResult();
    }
}

public record CreateBuyerRequest(int UserId, string CompanyName);
public record UpdateBuyerRequest(int UserId, string CompanyName);