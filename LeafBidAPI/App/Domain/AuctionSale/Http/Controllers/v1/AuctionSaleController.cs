using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.AuctionSale.Data;
using LeafBidAPI.App.Domain.AuctionSale.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.AuctionSale.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.AuctionSale.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleController(
    ApplicationDbContext context,
    AuctionSaleRepository auctionSaleRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all auction sales
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSaleResource>>> GetAuctionSales()
    {
        var sales = await Context.AuctionSales
            .AsNoTracking()
            .ProjectTo<AuctionSaleResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(sales) { StatusCode = 200 };
    }

    /// <summary>
    /// Get auction sale by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSaleResource>> GetAuctionSale(int id)
    {
        var result = await auctionSaleRepository.GetAuctionSaleAsync(new GetAuctionSaleData(id));

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<AuctionSaleResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auction sale
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSaleResource>> CreateAuctionSale([FromBody] CreateAuctionSaleRequest request)
    {
        var result = await auctionSaleRepository.CreateAuctionSaleAsync(
            new CreateAuctionSaleData(
                request.AuctionId,
                request.BuyerId,
                request.Date,
                request.PaymentReference
            )
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<AuctionSaleResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSaleResource>> UpdateAuctionSale(int id, [FromBody] UpdateAuctionSaleRequest request)
    {
        var result = await auctionSaleRepository.UpdateAuctionSaleAsync(
            new UpdateAuctionSaleData(id, request.PaymentReference)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<AuctionSaleResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
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