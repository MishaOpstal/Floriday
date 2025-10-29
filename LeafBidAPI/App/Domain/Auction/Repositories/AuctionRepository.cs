using FluentResults;
using LeafBidAPI.App.Domain.Auction.Data;
using LeafBidAPI.App.Domain.Auction.Validators;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Repositories;

namespace LeafBidAPI.App.Domain.Auction.Repositories;

public class AuctionRepository(
    ApplicationDbContext dbContext,
    CreateAuctionValidator createAuctionValidator,
    GetAuctionValidator getAuctionValidator,
    UpdateAuctionValidator updateAuctionValidator,
    DeleteAuctionValidator deleteAuctionValidator) : BaseRepository
{
    public async Task<Result<Models.Auction>> GetAuctionAsync(GetAuctionData auctionData)
    {
        var validation = await ValidateAsync(getAuctionValidator, auctionData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Auction>();

        var auction = await dbContext.Auctions.FindAsync(auctionData.Id);
        return auction is null
            ? Result.Fail("Auction not found.")
            : Result.Ok(auction);
    }
    
    public async Task<Result<Models.Auction>> CreateAuctionAsync(CreateAuctionData auctionData)
    {
        var validation = await ValidateAsync(createAuctionValidator, auctionData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Auction>();

        var auction = new Models.Auction
        {
            Description = auctionData.Description,
            StartDate = auctionData.StartDate,
            Amount = auctionData.Amount,
            MinimumPrice = auctionData.MinimumPrice,
            ClockLocationEnum = auctionData.ClockLocationEnum,
            AuctioneerId = auctionData.AuctioneerId,
        };

        await dbContext.Auctions.AddAsync(auction);
        await dbContext.SaveChangesAsync();

        return Result.Ok(auction);
    }
    
    public async Task<Result<Models.Auction>> UpdateAuctionAsync(UpdateAuctionData auctionData)
    {
        var validation = await ValidateAsync(updateAuctionValidator, auctionData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Auction>();

        var auction = await dbContext.Auctions.FindAsync(auctionData.Id);
        if (auction is null)
            return Result.Fail("Auction not found.");
        
        auction.Description = auctionData.Description ?? auction.Description;
        auction.StartDate = auctionData.StartDate ?? auction.StartDate;
        auction.Amount = auctionData.Amount ?? auction.Amount;
        auction.MinimumPrice = auctionData.MinimumPrice ?? auction.MinimumPrice;
        auction.ClockLocationEnum = auctionData.ClockLocationEnum ?? auction.ClockLocationEnum;

        dbContext.Auctions.Update(auction);
        await dbContext.SaveChangesAsync();

        return Result.Ok(auction);
    }
    
    public async Task<Result> DeleteAuctionAsync(DeleteAuctionData auctionData)
    {
        var validation = await ValidateAsync(deleteAuctionValidator, auctionData);
        if (validation.IsFailed)
            return validation;

        var auction = await dbContext.Auctions.FindAsync(auctionData.Id);
        if (auction is null)
            return Result.Fail("Auction not found.");

        dbContext.Auctions.Remove(auction);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}