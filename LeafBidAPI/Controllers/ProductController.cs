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
        product.Stock = updatedProducts.Stock;
        product.Auction = updatedProducts.Auction;

        if (updatedProducts.PotSize.HasValue)
        {
            product.PotSize = updatedProducts.PotSize;
            updatedProducts.StemLength = null;
        } else if (updatedProducts.StemLength.HasValue)
        {
            product.StemLength = updatedProducts.StemLength;
            updatedProducts.PotSize = null;       
        }
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(product);
    }
}