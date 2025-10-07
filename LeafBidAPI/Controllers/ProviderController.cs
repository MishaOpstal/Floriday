using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProviderController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    /// <summary>
    /// Get all providers.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Provider>>> GetProviders()
    {
        return await DbContext.Providers.ToListAsync();
    }

    /// <summary>
    /// Get a provider by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Provider>> GetProvider(int id)
    {
        var provider = await DbContext.Providers.FindAsync(id);
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
        DbContext.Providers.Add(provider);
        await DbContext.SaveChangesAsync();

        return new JsonResult(provider) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing provider by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProvider(int id, Provider updatedProvider)
    {
        var provider = await DbContext.Providers.FindAsync(id);
        if (provider == null)
        {
            return NotFound();
        }

        provider.CompanyName = updatedProvider.CompanyName;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(provider);
    }
    
    /// <summary>
    /// Delete a provider by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProvider(int id)
    {
        var provider = await DbContext.Providers.FindAsync(id);
        if (provider == null)
        {
            return NotFound();
        }

        DbContext.Providers.Remove(provider);
        await DbContext.SaveChangesAsync();
        return new OkResult();
    }
}