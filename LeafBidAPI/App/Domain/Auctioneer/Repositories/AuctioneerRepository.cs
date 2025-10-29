using FluentResults;
using LeafBidAPI.App.Domain.Auctioneer.Data;
using LeafBidAPI.App.Domain.Auctioneer.Validators;
using LeafBidAPI.App.Infrastructure.Common.Repositories;
using LeafBidAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Auctioneer.Repositories;

public class AuctioneerRepository(
    ApplicationDbContext dbContext,
    CreateAuctioneerValidator createAuctioneerValidator,
    GetAuctioneerValidator getAuctioneerValidator,
    DeleteAuctioneerValidator deleteAuctioneerValidator) : BaseRepository
{
    public async Task<Result<Models.Auctioneer>> GetAuctioneerAsync(GetAuctioneerData auctioneerData)
    {
        var validation = await ValidateAsync(getAuctioneerValidator, auctioneerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Auctioneer>();

        var auctioneer = await dbContext.Auctioneers.FindAsync(auctioneerData.Id);
        return auctioneer is null
            ? Result.Fail("Auctioneer not found.")
            : Result.Ok(auctioneer);
    }
    
    public async Task<Result<Models.Auctioneer>> CreateAuctioneerAsync(CreateAuctioneerData auctioneerData)
    {
        var validation = await ValidateAsync(createAuctioneerValidator, auctioneerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Auctioneer>();
        
        // Prevent duplicate auctioneer records
        bool exists = await dbContext.Auctioneers.AnyAsync(a => a.UserId == auctioneerData.UserId);
        if (exists)
            return Result.Fail("User is already an auctioneer.");

        var auctioneer = new Models.Auctioneer
        {
            UserId = auctioneerData.UserId,
        };

        await dbContext.Auctioneers.AddAsync(auctioneer);
        await dbContext.SaveChangesAsync();

        return Result.Ok(auctioneer);
    }
    
    public async Task<Result> DeleteBuyerAsync(DeleteAuctioneerData auctioneerData)
    {
        var validation = await ValidateAsync(deleteAuctioneerValidator, auctioneerData);
        if (validation.IsFailed)
            return validation;

        var auctioneer = await dbContext.Auctioneers.FindAsync(auctioneerData.Id);
        if (auctioneer is null)
            return Result.Fail("Auctioneer not found.");

        dbContext.Auctioneers.Remove(auctioneer);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}