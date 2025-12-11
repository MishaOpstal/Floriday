using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
[Authorize]
// [AllowAnonymous]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return Ok (await productService.GetProducts());
    }

    /// <summary>
    /// Get all available products
    /// </summary>
    [HttpGet("available")]
    public async Task<ActionResult<List<Product>>> GetAvailableProducts()
    {
        return Ok (await productService.GetAvailableProducts());
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(int id)
    {
        ProductResponse product =  await productService.GetProductById(id);
        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productData)
    {
        Product product = await productService.CreateProduct(productData);
        return Ok(productService.CreateProductResponse(product));
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Product>> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDto updatedProduct)
    {
        Product product = await productService.UpdateProduct(id, updatedProduct);
        return Ok(productService.CreateProductResponse(product));
    }

    /// <summary>
    /// Delete a product by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        bool deleted = await productService.DeleteProduct(id);
        return deleted ? NoContent() : Problem("Product deletion failed.");
    }

    [NonAction]
    public ProductResponse CreateProductResponse(Product product)
    {
        return productService.CreateProductResponse(product);
    }
}