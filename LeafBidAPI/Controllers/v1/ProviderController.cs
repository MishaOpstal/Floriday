using LeafBidAPI.Data;
using LeafBidAPI.Domain.Provider.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProviderController(ApplicationDbContext context) : BaseController(context)
{
    /// <summary>
    /// Get all providers.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Provider>>> GetProviders()
    {
        return await Context.Providers.ToListAsync();
    }

    /// <summary>
    /// Get a provider by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Provider>> GetProvider(int id)
    {
        var provider = await Context.Providers.FindAsync(id);
        if (provider == null)
        {
            return NotFound();
        }

        return provider;
    }
    
    /// <summary>
    /// Create a new provider.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Provider>> CreateProvider(Provider provider)
    {
        Context.Providers.Add(provider);
        await Context.SaveChangesAsync();

        return new JsonResult(provider) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing provider by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProvider(int id, Provider updatedProvider)
    {
        var provider = await Context.Providers.FindAsync(id);
        if (provider == null)
        {
            return NotFound();
        }

        provider.CompanyName = updatedProvider.CompanyName;
        
        await Context.SaveChangesAsync();
        return new JsonResult(provider);
    }
    
    /// <summary>
    /// Delete a provider by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProvider(int id)
    {
        var provider = await Context.Providers.FindAsync(id);
        if (provider == null)
        {
            return NotFound();
        }

        Context.Providers.Remove(provider);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}