using LeafBidAPI.App.Domain.AuctionSaleProduct.Data;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class AuctionSaleProductController(ApplicationDbContext context, AuctionSaleProductRepository auctionSaleProductRepository) : BaseController(context)
{
    /// <summary>
    /// Get all auction sale products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.AuctionSaleProduct>>> GetAuctionSaleProducts()
    {
        return await Context.AuctionSaleProducts.ToListAsync();
    }

    /// <summary>
    /// Get an auction sale product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.AuctionSaleProduct>> GetAuctionSaleProduct(int id)
    {
        var auctionSaleProduct = await auctionSaleProductRepository.GetAuctionSaleProductAsync(
            new GetAuctionSaleProductData(id)
        );
        
        return auctionSaleProduct.IsFailed ? NotFound() : new JsonResult(auctionSaleProduct.Value) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new auction sale product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.AuctionSaleProduct>> CreateAuctionSaleProducts([FromBody] CreateAuctionSaleProductRequest request)
    {
        var auctionSaleProduct = await auctionSaleProductRepository.CreateAuctionSaleProductAsync(
            new CreateAuctionSaleProductData(
                request.AuctionSaleId,
                request.ProductId,
                request.Quantity,
                request.Price
            )
        );
        
        return auctionSaleProduct.IsFailed ? BadRequest(auctionSaleProduct.Errors) : new JsonResult(auctionSaleProduct.Value) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing auction sale product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Models.AuctionSaleProduct>> UpdateAuctionSaleProducts(
        int id, [FromBody] UpdateAuctionSaleProductRequest result)
    {
        var auctionSaleProduct = await auctionSaleProductRepository.UpdateAuctionSaleProductAsync(
            new UpdateAuctionSaleProductData(
                id,
                result.AuctionSaleId,
                result.ProductId,
                result.Quantity,
                result.Price
            )
        );
        
        return auctionSaleProduct.IsFailed ? BadRequest(auctionSaleProduct.Errors) : new JsonResult(auctionSaleProduct.Value) { StatusCode = 200 };
    }
}

public record CreateAuctionSaleProductRequest
(
    int AuctionSaleId,
    int ProductId,
    int Quantity,
    decimal Price
);

public record UpdateAuctionSaleProductRequest
(
    int AuctionSaleId,
    int ProductId,
    int Quantity,
    decimal Price
);