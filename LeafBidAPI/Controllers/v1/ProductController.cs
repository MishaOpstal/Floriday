using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
// [AllowAnonymous]
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
    /// Get all available products
    /// </summary>
    [HttpGet("available")]
    public async Task<ActionResult<List<Product>>> GetAvailableProducts()
    {
        return await Context.Products.Where(p => p.AuctionId == null).ToListAsync();
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
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productData)
    {
        if (!string.IsNullOrEmpty(productData.Picture) && productData.Picture.StartsWith("data:image"))
        {
            try
            {
                string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                // Strip prefix "data:image/...;base64,"
                string? base64Data = productData.Picture.Contains(',')
                    ? productData.Picture[(productData.Picture.IndexOf(',') + 1)..]
                    : productData.Picture;

                byte[] bytes = Convert.FromBase64String(base64Data);
                string fileName = $"{Guid.NewGuid()}.png";
                string filePath = Path.Combine(uploadsDir, fileName);

                using (Image image = Image.Load(bytes))
                {
                    const int targetSize = 800;

                    // --- Step 1: compute zoom scaling ---
                    double scale = Math.Max(
                        (double)targetSize / image.Width,
                        (double)targetSize / image.Height
                    );

                    int resizedWidth = (int)(image.Width * scale);
                    int resizedHeight = (int)(image.Height * scale);

                    image.Mutate(x => x.Resize(resizedWidth, resizedHeight));

                    // --- Step 2: center-crop to 800x800 ---
                    int cropX = (resizedWidth - targetSize) / 2;
                    int cropY = (resizedHeight - targetSize) / 2;

                    Rectangle cropRect = new(cropX, cropY, targetSize, targetSize);

                    image.Mutate(x => x.Crop(cropRect));

                    // Save as PNG
                    await image.SaveAsPngAsync(filePath);
                }

                productData.Picture = $"/uploads/{fileName}";
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        Product product = new Product
        {
            Name = productData.Name,
            Description = productData.Description,
            MinPrice = productData.MinPrice,
            Weight = productData.Weight,
            Picture = productData.Picture,
            Species = productData.Species,
            Region = productData.Region,
            PotSize = productData.PotSize,
            StemLength = productData.StemLength,
            Stock = productData.Stock,
            HarvestedAt = productData.HarvestedAt,
            UserId = productData.UserId,
            AuctionId = productData.AuctionId
        };

        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        return new JsonResult(product) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Product>> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductDto updatedProduct)
    {
        Product? product = await Context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
        {
            return NotFound();
        }

        product.Name = updatedProduct.Name;
        product.Description = updatedProduct.Description;
        product.MinPrice = updatedProduct.MinPrice;
        product.Weight = updatedProduct.Weight;
        product.Picture = updatedProduct.Picture;
        product.Species = updatedProduct.Species;
        product.Region = updatedProduct.Region;
        product.Stock = updatedProduct.Stock;
        product.HarvestedAt = updatedProduct.HarvestedAt;
        product.AuctionId = updatedProduct.AuctionId;

        if (updatedProduct.PotSize.HasValue)
        {
            product.PotSize = updatedProduct.PotSize;
            updatedProduct.StemLength = null;
        }
        else if (updatedProduct.StemLength.HasValue)
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
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        Product? product = await Context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
        {
            return NotFound();
        }

        Context.Products.Remove(product);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}