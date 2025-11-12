using Azure;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class PagesController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<Page>> getPage (int id)
    {
        AuctionController auctionC = new AuctionController();
        var page = await 
        
        
        return page;
    }
}