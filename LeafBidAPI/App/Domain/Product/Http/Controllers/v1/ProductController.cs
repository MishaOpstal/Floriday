using LeafBidAPI.App.Domain.Product.Data;
using LeafBidAPI.App.Domain.Product.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Product.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductController(ApplicationDbContext context, ProductRepository productRepository) : BaseController(context)
{
    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.Product>>> GetProducts()
    {
        return await Context.Products.ToListAsync();
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Product>> GetProduct(int id)
    {
        var product = await productRepository.GetProductAsync(
            new Data.GetProductData(id)
        );
        
        return product.IsFailed ? NotFound() : new JsonResult(product.Value) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.Product>> CreateProduct([FromBody] CreateProductRequest request)
    {
        var product = await productRepository.CreateProductAsync(
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
        
        return product.IsFailed
            ? BadRequest(product.Errors)
            : new JsonResult(product.Value) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest updatedProduct)
    {
        var product = await productRepository.UpdateProductAsync(
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
        
        return product.IsFailed ? NotFound() : new JsonResult(product.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Delete a product by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var result = await productRepository.DeleteProductAsync(
            new DeleteProductData(id)
        );
        
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