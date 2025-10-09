using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ProductController(ApplicationDbContext context) : BaseController(context)
{
    /// <summary>
    /// Get all products
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return await Context.Products.ToListAsync();
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await Context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        return new JsonResult(product) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product updatedProduct)
    {
        var product = await Context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = updatedProduct.Name;
        product.Weight = updatedProduct.Weight;
        product.Picture = updatedProduct.Picture;
        product.Species = updatedProduct.Species;
        product.Stock = updatedProduct.Stock;
        product.Auction = updatedProduct.Auction;

        if (updatedProduct.PotSize.HasValue)
        {
            product.PotSize = updatedProduct.PotSize;
            updatedProduct.StemLength = null;
        } else if (updatedProduct.StemLength.HasValue)
        {
            product.StemLength = updatedProduct.StemLength;
            updatedProduct.PotSize = null;       
        }
        
        await Context.SaveChangesAsync();
        return new JsonResult(product);
    }
    
    /// <summary>
    /// Delete a product by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await Context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        Context.Products.Remove(product);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}