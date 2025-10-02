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
            return NotFound();

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
    public async Task<ActionResult> UpdateProduct(int id, Product updatedProducts)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        
        product.Name = updatedProducts.Name;
        product.Weight = updatedProducts.Weight;
        product.Picture = updatedProducts.Picture;
        product.Species = updatedProducts.Species;
        product.PotSize = updatedProducts.PotSize;
        product.StemLength = updatedProducts.StemLength;
        product.Stock = updatedProducts.Stock;
        product.Auction = updatedProducts.Auction;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(product);
    }
}