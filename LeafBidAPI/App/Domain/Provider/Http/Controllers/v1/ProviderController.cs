using LeafBidAPI.App.Domain.Provider.Data;
using LeafBidAPI.App.Domain.Provider.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Provider.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProviderController(ApplicationDbContext context, ProviderRepository providerRepository) : BaseController(context)
{
    /// <summary>
    /// Get all providers.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.Provider>>> GetProviders()
    {
        return await Context.Providers.ToListAsync();
    }

    /// <summary>
    /// Get a provider by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Provider>> GetProvider(int id)
    {
        var provider = await providerRepository.GetProviderAsync(
            new GetProviderData(id)
        );
        
        return provider.IsFailed ? NotFound() : new JsonResult(provider.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Create a new provider.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.Provider>> CreateProvider([FromBody] CreateProviderRequest request)
    {
        var provider = await providerRepository.CreateProviderAsync(
            new CreateProviderData(
                request.UserId,
                request.CompanyName
            )
        );
        
        return provider.IsFailed ? BadRequest(provider.Errors) : new JsonResult(provider.Value) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing provider by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProvider(int id, [FromBody] UpdateProviderRequest providerData)
    {
        var provider = await providerRepository.UpdateProviderAsync(
            new UpdateProviderData(
                id,
                providerData.UserId,
                providerData.CompanyName
            )
        );
        
        return provider.IsFailed ? BadRequest(provider.Errors) : new JsonResult(provider.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Delete a provider by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProvider(int id)
    {
        var result = await providerRepository.DeleteProviderAsync(
            new DeleteProviderData(id)
        );
        
        return result.IsFailed ? BadRequest(result.Errors) : new OkResult();
    }
}

public record CreateProviderRequest(int UserId, string CompanyName);
public record UpdateProviderRequest(int UserId, string CompanyName);