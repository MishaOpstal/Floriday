using LeafBidAPI.Data;
using LeafBidAPI.DTOs.Product;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using Rectangle = SixLabors.ImageSharp.Rectangle;

namespace LeafBidAPI.Services;

public class ProductService(ApplicationDbContext context) : IProductService
{

    public async Task<List<Product>> GetProducts()
    {
        return await context.Products.ToListAsync();
    }

    public async Task<List<Product>> GetAvailableProducts()
    {
        List<Product> products = await context.Products
            .Where(p => !context.AuctionProducts.Any(ap => ap.ProductId == p.Id))
            .ToListAsync();

        return products;
    }

    public async Task<Product> GetProductById(int id)
    {

        Product? product = await context.Products.Where(p => p.Id == id).Include(product => product.User).FirstOrDefaultAsync();
        return product ?? throw new NotFoundException("Product not found");
    }

    public async Task<Product> CreateProduct(CreateProductDto productData)
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
                throw new Exception("Failed to process image", ex);
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

        context.Products.Add(product);
        await context.SaveChangesAsync();

        return product;
    }

    public async Task<Product> UpdateProduct(int id, UpdateProductDto updatedProduct)
    {

        Product? product = await context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
        {
            throw new NotFoundException("Product not found");
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
            product.StemLength = null;
        }
        else if (updatedProduct.StemLength.HasValue)
        {
            product.StemLength = updatedProduct.StemLength;
            product.PotSize = null;
        }

        await context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        Product? product = await context.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
        if (product == null)
        {
            return false;
        }

        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return true;
    }

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