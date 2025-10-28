using LeafBidAPI.App.Domain.Buyer.Entities;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BuyerController(ApplicationDbContext context) : BaseController(context)
{
    
    /// <summary>
    /// Get all buyers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Buyer>>> GetBuyers()
    {
        return await Context.Buyers.ToListAsync();
    }

    /// <summary>
    /// Get a buyer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Buyer>> GetBuyer(int id)
    {
        var buyer = await Context.Buyers.FindAsync(id);
        if (buyer == null)
        {
            return NotFound();
        }

        return buyer;
    }

    /// <summary>
    /// Create a new buyer
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Buyer>> CreateBuyer(Buyer buyer)
    {
        Context.Buyers.Add(buyer);
        await Context.SaveChangesAsync();

        return new JsonResult(buyer) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing buyer
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBuyer(int id, Buyer updatedBuyer)
    {
        var buyer = await Context.Buyers.FindAsync(id);
        if (buyer == null)
        {
            return NotFound();
        }

        buyer.CompanyName = updatedBuyer.CompanyName;
        
        await Context.SaveChangesAsync();
        return new JsonResult(buyer);
    }
    
    /// <summary>
    /// Delete a buyer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBuyer(int id)
    {
        var buyer = await Context.Buyers.FindAsync(id);
        if (buyer == null)
        {
            return NotFound();
        }

        Context.Buyers.Remove(buyer);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}