using FluentResults;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Data;
using LeafBidAPI.App.Domain.AuctionSaleProduct.Validators;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Repositories;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Repositories;

public class AuctionSaleProductRepository(
    ApplicationDbContext dbContext,
    CreateAuctionSaleProductValidator createAuctionSaleProductValidator,
    GetAuctionSaleProductValidator getAuctionSaleProductValidator,
    UpdateAuctionSaleProductValidator updateAuctionSaleProductValidator) : BaseRepository
{
    public async Task<Result<Models.AuctionSaleProduct>> GetAuctionSaleProductAsync(GetAuctionSaleProductData buyerData)
    {
        var validation = await ValidateAsync(getAuctionSaleProductValidator, buyerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.AuctionSaleProduct>();

        var buyer = await dbContext.AuctionSaleProducts.FindAsync(buyerData.Id);
        return buyer is null
            ? Result.Fail("AuctionSaleProduct not found.")
            : Result.Ok(buyer);
    }
    
    public async Task<Result<Models.AuctionSaleProduct>> CreateAuctionSaleProductAsync(CreateAuctionSaleProductData buyerData)
    {
        var validation = await ValidateAsync(createAuctionSaleProductValidator, buyerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.AuctionSaleProduct>();

        var buyer = new Models.AuctionSaleProduct
        {
            AuctionSaleId = buyerData.AuctionSaleId,
            ProductId = buyerData.ProductId,
            Quantity = buyerData.Quantity,
            Price = buyerData.Price,
        };

        await dbContext.AuctionSaleProducts.AddAsync(buyer);
        await dbContext.SaveChangesAsync();

        return Result.Ok(buyer);
    }
    
    public async Task<Result<Models.AuctionSaleProduct>> UpdateAuctionSaleProductAsync(UpdateAuctionSaleProductData buyerData)
    {
        var validation = await ValidateAsync(updateAuctionSaleProductValidator, buyerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.AuctionSaleProduct>();

        var buyer = await dbContext.AuctionSaleProducts.FindAsync(buyerData.Id);
        if (buyer is null)
            return Result.Fail("AuctionSaleProduct not found.");
        
        buyer.Quantity = buyerData.Quantity ?? buyer.Quantity;
        buyer.Price = buyerData.Price ?? buyer.Price;

        dbContext.AuctionSaleProducts.Update(buyer);
        await dbContext.SaveChangesAsync();

        return Result.Ok(buyer);
    }
}