using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


namespace LeafBidAPI.Controllers.v1;


/// <summary>
/// get auction and product from the input ID
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Identity.Bearer")]
public class PagesController(ApplicationDbContext dbContext) : BaseController(dbContext)
{

    /// <summary>
    /// get auction and product by AuctionId
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProducts(int id)
    { 
        // roep alle endpoints aan tegelijkertijd
        Auction? auction = await dbContext.Auctions.FindAsync(id);
        List<Product> products = await dbContext.Products
            .Where(p => p.AuctionId == id)
            .ToListAsync();

        if (auction == null || products.IsNullOrEmpty())
        {
            return NotFound("Auction or product not found. Auction data: " + auction + ", Product data: " + products);
        }

        var result = new GetAuctionWithProductsDto
        {
            auction = auction,
            products = products
        };

        return new JsonResult(result);
    }
}