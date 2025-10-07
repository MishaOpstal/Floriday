using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return await DbContext.Products.ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        DbContext.Products.Add(product);
        await DbContext.SaveChangesAsync();

        return new JsonResult(product) { StatusCode = 201 };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product updatedProduct)
    {
        var product = await DbContext.Products.FindAsync(id);
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
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(product);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        DbContext.Products.Remove(product);
        await DbContext.SaveChangesAsync();
        return new OkResult();
    }
}