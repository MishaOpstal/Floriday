using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.Provider.Data;
using LeafBidAPI.App.Domain.Provider.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.Provider.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Provider.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProviderController(
    ApplicationDbContext context,
    ProviderRepository providerRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all providers.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProviderResource>>> GetProviders()
    {
        var providers = await Context.Providers
            .AsNoTracking()
            .ProjectTo<ProviderResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(providers) { StatusCode = 200 };
    }

    /// <summary>
    /// Get a provider by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProviderResource>> GetProvider(int id)
    {
        var result = await providerRepository.GetProviderAsync(new GetProviderData(id));

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<ProviderResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new provider.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProviderResource>> CreateProvider([FromBody] CreateProviderRequest request)
    {
        var result = await providerRepository.CreateProviderAsync(
            new CreateProviderData(request.UserId, request.CompanyName)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<ProviderResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing provider by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProviderResource>> UpdateProvider(int id, [FromBody] UpdateProviderRequest providerData)
    {
        var result = await providerRepository.UpdateProviderAsync(
            new UpdateProviderData(id, providerData.UserId, providerData.CompanyName)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<ProviderResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Delete a provider by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProvider(int id)
    {
        var result = await providerRepository.DeleteProviderAsync(new DeleteProviderData(id));

        return result.IsFailed ? BadRequest(result.Errors) : new OkResult();
    }
}

public record CreateProviderRequest(int UserId, string CompanyName);
public record UpdateProviderRequest(int UserId, string CompanyName);