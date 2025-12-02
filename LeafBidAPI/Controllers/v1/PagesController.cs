using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

/// <summary>
/// get auction and product from the input ID
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class PagesController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    /// <summary>
    /// get auction and product by AuctionId
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProducts(int id)
    {
        // Get the Auctions and the Products within the Auctions.
        Auction? auction = await Context.Auctions.FindAsync(id);
        List<Product> products = await Context.Products
            .Where(p => p.AuctionId == id)
            .ToListAsync();

        if (auction == null || products.Count == 0)
        {
            return NotFound("Auction or product not found. Auction data: " + auction + ", Product data: " + products);
        }

        GetAuctionWithProductsDto result = new()
        {
            Auction = auction,
            Products = products
        };

        return new JsonResult(result);
    }
}