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
        List<Product> products = await Context.Products
            .Where(p => p.Stock > 0 && !Context.AuctionProducts.Any(ap => ap.ProductId == p.Id))
            .ToListAsync();

        return products;
    }

    /// <summary>
    /// Get a product by id
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetProduct(int id)
    {
        Product? product = await Context.Products.Where(p => p.Id == id).Include(product => product.User).FirstOrDefaultAsync();
        return product == null ? NotFound() : CreateProductResponse(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Provider")]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductDto productData)
    {
        if (productData.HarvestedAt > DateTime.UtcNow)
        {
            return new JsonResult("Harvest date cannot be in the future")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
        }
        
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
        
        Product product = new()
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
        };

        Context.Products.Add(product);
        await Context.SaveChangesAsync();

        return new JsonResult(CreateProductResponse(product)) { StatusCode = 201 };
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
            return new JsonResult("Product not found")
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        
        if (updatedProduct.HarvestedAt > DateTime.UtcNow)
        {
            return new JsonResult("Harvest date cannot be in the future")
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
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
        return new JsonResult(CreateProductResponse(product)) { StatusCode = 200 };
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

    [NonAction]
    public ProductResponse CreateProductResponse(Product product)
    {
        ProductResponse productResponse = new()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            MinPrice = product.MinPrice,
            MaxPrice = product.MaxPrice,
            Weight = product.Weight,
            Picture = product.Picture,
            Species = product.Species,
            Region = product.Region,
            PotSize = product.PotSize,
            StemLength = product.StemLength,
            Stock = product.Stock,
            HarvestedAt = product.HarvestedAt,
            ProviderUserName = product.User?.UserName ?? ""
        };

        return productResponse;
    }
}