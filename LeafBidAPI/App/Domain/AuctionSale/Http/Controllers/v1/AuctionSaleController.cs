using LeafBidAPI.App.Domain.AuctionSale.Data;
using LeafBidAPI.App.Domain.AuctionSale.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.AuctionSale.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleController(ApplicationDbContext context, AuctionSaleRepository auctionSaleRepository) : BaseController(context)
{
    
    /// <summary>
    /// Get all auction sales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.AuctionSale>>> GetAuctionSales()
    {
        return await Context.AuctionSales.ToListAsync();
    }

    /// <summary>
    /// Get auction sale by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.AuctionSale>> GetAuctionSale(int id)
    {
        var auctionSale = await auctionSaleRepository.GetAuctionSaleAsync(
            new GetAuctionSaleData(id)
        );
        
        return auctionSale.IsFailed ? NotFound() : new JsonResult(auctionSale.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Create a new auction sale
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.AuctionSale>> CreateAuctionSale([FromBody] CreateAuctionSaleRequest request)
    {
        var auctionSale = await auctionSaleRepository.CreateAuctionSaleAsync(
            new CreateAuctionSaleData(
                request.AuctionId,
                request.BuyerId,
                request.Date,
                request.PaymentReference
            )
        );
        
        return auctionSale.IsFailed ? BadRequest(auctionSale.Errors) : new JsonResult(auctionSale.Value) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuctionSale(int id, [FromBody] UpdateAuctionSaleRequest request)
    {
        var auctionSale = await auctionSaleRepository.UpdateAuctionSaleAsync(
            new UpdateAuctionSaleData(
                id,
                request.PaymentReference
            )
        );

        return auctionSale.IsFailed
            ? BadRequest(auctionSale.Errors)
            : new JsonResult(auctionSale.Value) { StatusCode = 200 };
    }
}

public record CreateAuctionSaleRequest(
    int AuctionId,
    int BuyerId,
    DateTime Date,
    string PaymentReference
);

public record UpdateAuctionSaleRequest(
    string PaymentReference
);