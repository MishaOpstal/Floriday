using FluentResults;
using LeafBidAPI.App.Domain.AuctionSale.Data;
using LeafBidAPI.App.Domain.AuctionSale.Validators;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Repositories;

namespace LeafBidAPI.App.Domain.AuctionSale.Repositories;

public class AuctionSaleRepository(
    ApplicationDbContext dbContext,
    CreateAuctionSaleValidator createAuctionSaleValidator,
    GetAuctionSaleValidator getAuctionSaleValidator,
    UpdateAuctionSaleValidator updateAuctionSaleValidator) : BaseRepository
{
    public async Task<Result<Models.AuctionSale>> GetAuctionSaleAsync(GetAuctionSaleData auctionSaleData)
    {
        var validation = await ValidateAsync(getAuctionSaleValidator, auctionSaleData);
        if (validation.IsFailed)
            return validation.ToResult<Models.AuctionSale>();

        var auctionSale = await dbContext.AuctionSales.FindAsync(auctionSaleData.Id);
        return auctionSale is null
            ? Result.Fail("AuctionSale not found.")
            : Result.Ok(auctionSale);
    }
    
    public async Task<Result<Models.AuctionSale>> CreateAuctionSaleAsync(CreateAuctionSaleData auctionSaleData)
    {
        var validation = await ValidateAsync(createAuctionSaleValidator, auctionSaleData);
        if (validation.IsFailed)
            return validation.ToResult<Models.AuctionSale>();

        var auctionSale = new Models.AuctionSale
        {
            AuctionId = auctionSaleData.AuctionId,
            BuyerId = auctionSaleData.BuyerId,
            Date = auctionSaleData.Date,
            PaymentReference = auctionSaleData.PaymentReference,
        };

        await dbContext.AuctionSales.AddAsync(auctionSale);
        await dbContext.SaveChangesAsync();

        return Result.Ok(auctionSale);
    }
    
    public async Task<Result<Models.AuctionSale>> UpdateAuctionSaleAsync(UpdateAuctionSaleData auctionSaleData)
    {
        var validation = await ValidateAsync(updateAuctionSaleValidator, auctionSaleData);
        if (validation.IsFailed)
            return validation.ToResult<Models.AuctionSale>();

        var auctionSale = await dbContext.AuctionSales.FindAsync(auctionSaleData.Id);
        if (auctionSale is null)
            return Result.Fail("AuctionSale not found.");
        
        auctionSale.PaymentReference = auctionSaleData.PaymentReference;

        dbContext.AuctionSales.Update(auctionSale);
        await dbContext.SaveChangesAsync();

        return Result.Ok(auctionSale);
    }
}