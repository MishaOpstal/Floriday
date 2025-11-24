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

    /// <summary>
    /// Get 4 ongoing auctions
    /// <summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetPageDto>>> GetAuctions()
    {
        List<GetAuctionByClockEnumDto> AuctionList = new List<GetAuctionByClockEnumDto>();

        // verkrijg dynamisch het aantal clocklocation enums
        foreach (ClockLocationEnum clock in Enum.GetValues(typeof(ClockLocationEnum)))
        {
            var result = await dbContext.Auctions
                .Where(a =>
                    a.ClockLocationEnum == clock)
                .ToListAsync();

            foreach (var a in result)
            {
                Res
            }
        }

        return new JsonResult(resultList);
    }
}