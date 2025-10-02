using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BuyerController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<Buyer>>> GetBuyers()
    {
        return await DbContext.Buyers.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Buyer>> GetBuyer(int id)
    {
        var buyer = await DbContext.Buyers.FindAsync(id);
        if (buyer == null)
            return NotFound();

        return buyer;
    }

    [HttpPost]
    public async Task<ActionResult<Buyer>> CreateBuyer(Buyer buyer)
    {
        DbContext.Buyers.Add(buyer);
        await DbContext.SaveChangesAsync();

        return new JsonResult(buyer) { StatusCode = 201 };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateBuyer(int id, Buyer updatedBuyer)
    {
        var buyer = await DbContext.Buyers.FindAsync(id);
        if (buyer == null)
            return NotFound();
        
        buyer.CompanyName = updatedBuyer.CompanyName;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(buyer);
    }
}