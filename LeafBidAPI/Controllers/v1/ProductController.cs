using LeafBidAPI.Data;
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
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        if (!string.IsNullOrEmpty(product.Picture) && product.Picture.StartsWith("data:image"))
        {
            try
            {
                string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                // Strip prefix "data:image/...;base64,"
                string? base64Data = product.Picture.Contains(',')
                    ? product.Picture[(product.Picture.IndexOf(',') + 1)..]
                    : product.Picture;

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

                product.Picture = $"/uploads/{fileName}";
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
    [Authorize(Roles = "Provider")]
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
        product.Value.AuctionId = updatedProduct.AuctionId;

        if (updatedProduct.PotSize.HasValue)
        {
            product.Value.PotSize = updatedProduct.PotSize;
            updatedProduct.StemLength = null;
        }
        else if (updatedProduct.StemLength.HasValue)
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
    [Authorize(Roles = "Provider")]
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