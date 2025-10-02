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
        var Product = await DbContext.Products.FindAsync(id);
        if (Product == null)
            return NotFound();

        return Product;
    }

    [HttpPost]
    public async Task<ActionResult<Buyer>> CreateProduct(Product product)
    {
        DbContext.Products.Add(product);
        await DbContext.SaveChangesAsync();

        return new JsonResult(product) { StatusCode = 201 };
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product products)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        
        product.Name = products.Name;
        product.Weight = products.Weight;
        product.Picture = products.Picture;
        product.Species = products.Species;
        product.PotSize = products.PotSize;
        product.StemLength = products.StemLength;
        product.Stock = products.Stock;
        product.Auction = products.Auction;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(product);
    }
}