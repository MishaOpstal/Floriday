using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Page;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PagesController(ApplicationDbContext dbContext, IHttpClientFactory httpClientFactory) : BaseController(dbContext)
{

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPageDto>> getPage(int id)
    {        
        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("http://localhost:5001");
        

        // roep alle endpoints aan tegelijkertijd
        var auction = await dbContext.Auctions.FindAsync(id);
        var product = await dbContext.Products.FirstOrDefaultAsync(p => p.AuctionId == id);

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
}