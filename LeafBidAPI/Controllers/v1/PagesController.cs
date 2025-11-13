using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
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
    public async Task<ActionResult<GetAuctionWithProductsDto>> GetAuctionWithProducts(int id)
    { 
        // roep alle endpoints aan tegelijkertijd
        var auction = await dbContext.Auctions.FindAsync(id);
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.AuctionId == id);

        if (auction == null || product == null)
        {
            return NotFound();
        }

        var result = new GetAuctionWithProductsDto
        {
            auction = auction,
            product = product
        };

        return new JsonResult(result);
    }
}