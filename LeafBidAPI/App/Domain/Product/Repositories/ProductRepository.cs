using FluentResults;
using LeafBidAPI.App.Domain.Product.Data;
using LeafBidAPI.App.Domain.Product.Validators;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Repositories;

namespace LeafBidAPI.App.Domain.Product.Repositories;

public class ProductRepository(
    ApplicationDbContext dbContext,
    CreateProductValidator createProductValidator,
    GetProductValidator getProductValidator,
    UpdateProductValidator updateProductValidator,
    DeleteProductValidator deleteProductValidator) : BaseRepository
{
    public async Task<Result<Models.Product>> GetProductAsync(GetProductData productData)
    {
        var validation = await ValidateAsync(getProductValidator, productData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Product>();

        var product = await dbContext.Products.FindAsync(productData.Id);
        return product is null
            ? Result.Fail("Product not found.")
            : Result.Ok(product);
    }
    
    public async Task<Result<Models.Product>> CreateProductAsync(CreateProductData productData)
    {
        var validation = await ValidateAsync(createProductValidator, productData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Product>();

        var product = new Models.Product
        {
            AuctionId = productData.AuctionId,
            Name = productData.Name,
            Weight = productData.Weight,
            Picture = productData.Picture,
            Species = productData.Species,
            PotSize = productData.PotSize ?? 0,
            StemLength = productData.StemLength ?? 0,
            Stock = productData.Stock
        };

        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();

        return Result.Ok(product);
    }
    
    public async Task<Result<Models.Product>> UpdateProductAsync(UpdateProductData productData)
    {
        var validation = await ValidateAsync(updateProductValidator, productData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Product>();

        var product = await dbContext.Products.FindAsync(productData.Id);
        if (product is null)
            return Result.Fail("Product not found.");

        product.Name = productData.Name ?? product.Name;
        product.Weight = productData.Weight ?? product.Weight;
        product.Picture = productData.Picture ?? product.Picture;
        product.Species = productData.Species ?? product.Species;
        product.PotSize = productData.PotSize ?? product.PotSize;
        product.StemLength = productData.StemLength ?? product.StemLength;
        product.Stock = productData.Stock ?? product.Stock;

        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();

        return Result.Ok(product);
    }
    
    public async Task<Result> DeleteProductAsync(DeleteProductData productData)
    {
        var validation = await ValidateAsync(deleteProductValidator, productData);
        if (validation.IsFailed)
            return validation;

        var product = await dbContext.Products.FindAsync(productData.Id);
        if (product is null)
            return Result.Fail("Product not found.");

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}