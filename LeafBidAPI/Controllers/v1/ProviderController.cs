using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Identity.Bearer")]
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
        var provider = await Context.Providers.Where(p => p.Id == id).FirstOrDefaultAsync();
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
        var provider = await GetProvider(id);
        if (provider.Value == null)
        {
            return NotFound();
        }

        provider.Value.CompanyName = updatedProvider.CompanyName;
        
        await Context.SaveChangesAsync();
        return new JsonResult(provider.Value);
    }
    
    /// <summary>
    /// Delete a provider by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProvider(int id)
    {
        var provider = await GetProvider(id);
        if (provider.Value == null)
        {
            return NotFound();
        }

        Context.Providers.Remove(provider.Value);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}