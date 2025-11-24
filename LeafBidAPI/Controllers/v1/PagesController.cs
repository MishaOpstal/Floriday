using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Auction;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.Enums;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace LeafBidAPI.Controllers.v1;


/// <summary>
/// get auction and product from the input ID
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PagesController(ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory) : BaseController(dbContext)
{
    /// <summary>
    /// get auction and product by the same ID
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPageDto>> GetAuctionWithProducts(int id)
    { 
        // roep alle endpoints aan tegelijkertijd
        var auction = await dbContext.Auctions.FindAsync(id);
        var product = await dbContext.Products
            .Where(p => p.AuctionId == id)
            .ToListAsync();

        if (auction == null || product == null)
        {
            return NotFound();
        }

        var result = new GetPageDto
        {
            auction = auction,
            product = product
        };

        return new JsonResult(result);
    }

    /// <summary>
    /// Get 4 ongoing auctions
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetAuctionByClockEnumDto>>> GetAuctions()
    {
        var auctions = await dbContext.Auctions.ToListAsync();

        var products =  await dbContext.Products.ToListAsync();

        var productLookup = products
            .GroupBy(p=> p.AuctionId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var result = auctions
            .OrderBy(a => a.ClockLocationEnum) // optioneel: houdt dezelfde volgorde als enum
            .Select(a => new GetAuctionByClockEnumDto
            {
                auction = a,
                product = productLookup.ContainsKey(a.Id) ? productLookup[a.Id] : new List<Product>()
            })
            .ToList();

        return new JsonResult(result);
    }
}