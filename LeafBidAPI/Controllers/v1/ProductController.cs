using System.Drawing;
using System.Net;
using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

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
        var product = await Context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
        {
            return NotFound();
        }

        return product;
    }
    
    /// <summary>
    /// get a product by AuctionId
    /// </summary>
    [HttpGet("by-auctionId/{auctionId:int}")]
    public async Task<ActionResult<Product>> GetProductByAuctionId(int auctionId)
    {
        var product = await Context.Products.FirstOrDefaultAsync(p => p.AuctionId == auctionId);
        if (product ==  null)
        {
            return NotFound();
        }

        return product;
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        // Handle Base64 image upload if present
        if (!string.IsNullOrEmpty(product.Picture) && product.Picture.StartsWith("data:image"))
        {
            try
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                // Strip prefix "data:image/...;base64," if present
                var base64Data = product.Picture.Contains(',')
                    ? product.Picture[(product.Picture.IndexOf(',') + 1)..]
                    : product.Picture;

                var bytes = Convert.FromBase64String(base64Data);
                var fileName = $"{Guid.NewGuid()}.png";
                var filePath = Path.Combine(uploadsDir, fileName);

                // Decode and save using ImageSharp
                using (var image = SixLabors.ImageSharp.Image.Load(bytes))
                {
                    const int maxWidth = 800;
                    if (image.Width > maxWidth)
                    {
                        var ratio = (double)maxWidth / image.Width;
                        image.Mutate(x => x.Resize(maxWidth, (int)(image.Height * ratio)));
                    }

                    await image.SaveAsPngAsync(filePath);
                }

                product.Picture = $"/uploads/{fileName}";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

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
        var product = await GetProduct(id);
        if (product.Value == null) 
        {
            return NotFound();
        }

        product.Value.Name = updatedProduct.Name;
        product.Value.Weight = updatedProduct.Weight;
        product.Value.Picture = updatedProduct.Picture;
        product.Value.Species = updatedProduct.Species;
        product.Value.Stock = updatedProduct.Stock;
        product.Value.Auction = updatedProduct.Auction;

        if (updatedProduct.PotSize.HasValue)
        {
            product.Value.PotSize = updatedProduct.PotSize;
            updatedProduct.StemLength = null;
        } else if (updatedProduct.StemLength.HasValue)
        {
            product.Value.StemLength = updatedProduct.StemLength;
            updatedProduct.PotSize = null;       
        }
        
        await Context.SaveChangesAsync();
        return new JsonResult(product.Value);
    }
    
    /// <summary>
    /// Delete a product by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await GetProduct(id);
        if (product.Value == null)
        {
            return NotFound();
        }

        Context.Products.Remove(product.Value);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}