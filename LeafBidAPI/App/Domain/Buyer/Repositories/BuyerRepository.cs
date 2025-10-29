using FluentResults;
using LeafBidAPI.App.Domain.Buyer.Data;
using LeafBidAPI.App.Domain.Buyer.Validators;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Buyer.Repositories;

public class BuyerRepository(
    ApplicationDbContext dbContext,
    CreateBuyerValidator createBuyerValidator,
    GetBuyerValidator getBuyerValidator,
    UpdateBuyerValidator updateBuyerValidator,
    DeleteBuyerValidator deleteBuyerValidator) : BaseRepository
{
    public async Task<Result<Models.Buyer>> GetBuyerAsync(GetBuyerData buyerData)
    {
        var validation = await ValidateAsync(getBuyerValidator, buyerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Buyer>();

        var buyer = await dbContext.Buyers.FindAsync(buyerData.Id);
        return buyer is null
            ? Result.Fail("Buyer not found.")
            : Result.Ok(buyer);
    }
    
    public async Task<Result<Models.Buyer>> CreateBuyerAsync(CreateBuyerData buyerData)
    {
        var validation = await ValidateAsync(createBuyerValidator, buyerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Buyer>();
        
        // Prevent duplicate buyer records
        bool exists = await dbContext.Buyers.AnyAsync(b => b.UserId == buyerData.UserId);
        if (exists)
            return Result.Fail("User is already a buyer.");

        var buyer = new Models.Buyer
        {
            UserId = buyerData.UserId,
            CompanyName = buyerData.CompanyName,
        };

        await dbContext.Buyers.AddAsync(buyer);
        await dbContext.SaveChangesAsync();

        return Result.Ok(buyer);
    }
    
    public async Task<Result<Models.Buyer>> UpdateBuyerAsync(UpdateBuyerData buyerData)
    {
        var validation = await ValidateAsync(updateBuyerValidator, buyerData);
        if (validation.IsFailed)
            return validation.ToResult<Models.Buyer>();

        var buyer = await dbContext.Buyers.FindAsync(buyerData.Id);
        if (buyer is null)
            return Result.Fail("Buyer not found.");

        buyer.CompanyName = buyerData.CompanyName ?? buyer.CompanyName;

        dbContext.Buyers.Update(buyer);
        await dbContext.SaveChangesAsync();

        return Result.Ok(buyer);
    }
    
    public async Task<Result> DeleteBuyerAsync(DeleteBuyerData buyerData)
    {
        var validation = await ValidateAsync(deleteBuyerValidator, buyerData);
        if (validation.IsFailed)
            return validation;

        var buyer = await dbContext.Buyers.FindAsync(buyerData.Id);
        if (buyer is null)
            return Result.Fail("Buyer not found.");

        dbContext.Buyers.Remove(buyer);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}