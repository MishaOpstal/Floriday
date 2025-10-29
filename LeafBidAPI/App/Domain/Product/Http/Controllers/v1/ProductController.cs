using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.Product.Data;
using LeafBidAPI.App.Domain.Product.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.Product.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Product.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductController(
    ApplicationDbContext context,
    ProductRepository productRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<ProductResource>>> GetProducts()
    {
        var products = await Context.Products
            .AsNoTracking()
            .ProjectTo<ProductResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(products) { StatusCode = 200 };
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResource>> GetProduct(int id)
    {
        var result = await productRepository.GetProductAsync(new GetProductData(id));

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<ProductResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ProductResource>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var result = await productRepository.CreateProductAsync(
            new CreateProductData(
                request.Name,
                request.Weight,
                request.Picture,
                request.Species,
                request.PotSize,
                request.StemLength,
                request.Stock,
                request.AuctionId
            )
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<ProductResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResource>> UpdateProduct(int id, [FromBody] UpdateProductRequest updatedProduct)
    {
        var result = await productRepository.UpdateProductAsync(
            new UpdateProductData(
                id,
                updatedProduct.Name,
                updatedProduct.Weight,
                updatedProduct.Picture,
                updatedProduct.Species,
                updatedProduct.PotSize,
                updatedProduct.StemLength,
                updatedProduct.Stock,
                updatedProduct.AuctionId
            )
        );

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<ProductResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Delete a product by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await productRepository.DeleteProductAsync(new DeleteProductData(id));

        return result.IsFailed ? NotFound() : new OkResult();
    }
}

public record CreateProductRequest(
    string Name,
    double Weight,
    string Picture,
    string Species,
    double? PotSize,
    double? StemLength,
    int Stock,
    int AuctionId
);

public record UpdateProductRequest(
    string Name,
    double Weight,
    string Picture,
    string Species,
    double? PotSize,
    double? StemLength,
    int Stock,
    int AuctionId
);