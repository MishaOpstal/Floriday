using LeafBidAPI.Data;
using LeafBidAPI.Models;
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
        var buyer = await Context.Buyers.Where(b => b.Id == id).FirstOrDefaultAsync();
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
        var buyer = await GetBuyer(id);
        if (buyer.Value == null)
        {
            return NotFound();
        }

        buyer.Value.CompanyName = updatedBuyer.CompanyName;
        
        await Context.SaveChangesAsync();
        return new JsonResult(buyer.Value);
    }
    
    /// <summary>
    /// Delete a buyer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBuyer(int id)
    {
        var buyer = await GetBuyer(id);
        if (buyer.Value == null)
        {
            return NotFound();
        }

        Context.Buyers.Remove(buyer.Value);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}