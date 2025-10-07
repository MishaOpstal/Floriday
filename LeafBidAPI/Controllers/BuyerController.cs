using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuyerController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    
    /// <summary>
    /// Get all buyers
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Buyer>>> GetBuyers()
    {
        return await DbContext.Buyers.ToListAsync();
    }

    /// <summary>
    /// Get a buyer by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Buyer>> GetBuyer(int id)
    {
        var buyer = await DbContext.Buyers.FindAsync(id);
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
        DbContext.Buyers.Add(buyer);
        await DbContext.SaveChangesAsync();

        return new JsonResult(buyer) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing buyer
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBuyer(int id, Buyer updatedBuyer)
    {
        var buyer = await DbContext.Buyers.FindAsync(id);
        if (buyer == null)
        {
            return NotFound();
        }

        buyer.CompanyName = updatedBuyer.CompanyName;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(buyer);
    }
    
    /// <summary>
    /// Delete a buyer by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteBuyer(int id)
    {
        var buyer = await DbContext.Buyers.FindAsync(id);
        if (buyer == null)
        {
            return NotFound();
        }

        DbContext.Buyers.Remove(buyer);
        await DbContext.SaveChangesAsync();
        return new OkResult();
    }
}