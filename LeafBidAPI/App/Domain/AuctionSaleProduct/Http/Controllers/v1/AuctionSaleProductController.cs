using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Data;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.AuctionSaleProduct.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleProductController(
    ApplicationDbContext context,
    AuctionSaleProductRepository auctionSaleProductRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all auction sale products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<AuctionSaleProductResource>>> GetAuctionSaleProducts()
    {
        var products = await Context.AuctionSaleProducts
            .AsNoTracking()
            .ProjectTo<AuctionSaleProductResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(products) { StatusCode = 200 };
    }

    /// <summary>
    /// Get an auction sale product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuctionSaleProductResource>> GetAuctionSaleProduct(int id)
    {
        var result = await auctionSaleProductRepository.GetAuctionSaleProductAsync(
            new GetAuctionSaleProductData(id)
        );

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<AuctionSaleProductResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auction sale product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<AuctionSaleProductResource>> CreateAuctionSaleProduct([FromBody] CreateAuctionSaleProductRequest request)
    {
        var result = await auctionSaleProductRepository.CreateAuctionSaleProductAsync(
            new CreateAuctionSaleProductData(
                request.AuctionSaleId,
                request.ProductId,
                request.Quantity,
                request.Price
            )
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<AuctionSaleProductResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<AuctionSaleProductResource>> UpdateAuctionSaleProduct(
        int id, [FromBody] UpdateAuctionSaleProductRequest request)
    {
        var result = await auctionSaleProductRepository.UpdateAuctionSaleProductAsync(
            new UpdateAuctionSaleProductData(
                id,
                request.AuctionSaleId,
                request.ProductId,
                request.Quantity,
                request.Price
            )
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<AuctionSaleProductResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }
}

public record CreateAuctionSaleProductRequest(
    int AuctionSaleId,
    int ProductId,
    int Quantity,
    decimal Price
);

public record UpdateAuctionSaleProductRequest(
    int AuctionSaleId,
    int ProductId,
    int Quantity,
    decimal Price
);