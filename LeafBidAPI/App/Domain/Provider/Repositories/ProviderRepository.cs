using FluentResults;
using LeafBidAPI.App.Domain.Provider.Data;
using LeafBidAPI.App.Domain.Provider.Validators;
using LeafBidAPI.App.Infrastructure.Common.Repositories;
using LeafBidAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.Provider.Repositories;

public class ProviderRepository(
    ApplicationDbContext dbContext,
    CreateProviderValidator createProviderValidator,
    GetProviderValidator getProviderValidator,
    UpdateProviderValidator updateProviderValidator,
    DeleteProviderValidator deleteProviderValidator) : BaseRepository
{
    public async Task<Result<Entities.Provider>> GetProviderAsync(GetProviderData providerData)
    {
        var validation = await ValidateAsync(getProviderValidator, providerData);
        if (validation.IsFailed)
            return validation.ToResult<Entities.Provider>();

        var provider = await dbContext.Providers.FindAsync(providerData.Id);
        return provider is null
            ? Result.Fail("Provider not found.")
            : Result.Ok(provider);
    }
    
    public async Task<Result<Entities.Provider>> CreateProviderAsync(CreateProviderData providerData)
    {
        var validation = await ValidateAsync(createProviderValidator, providerData);
        if (validation.IsFailed)
            return validation.ToResult<Entities.Provider>();

        // Prevent duplicate provider records
        bool exists = await dbContext.Providers.AnyAsync(p => p.UserId == providerData.UserId);
        if (exists)
            return Result.Fail("User is already a provider.");

        var provider = new Entities.Provider
        {
            UserId = providerData.UserId,
            CompanyName = providerData.CompanyName
        };

        await dbContext.Providers.AddAsync(provider);
        await dbContext.SaveChangesAsync();

        return Result.Ok(provider);
    }
    
    public async Task<Result<Entities.Provider>> UpdateProviderAsync(UpdateProviderData providerData)
    {
        var validation = await ValidateAsync(updateProviderValidator, providerData);
        if (validation.IsFailed)
            return validation.ToResult<Entities.Provider>();

        var provider = await dbContext.Providers.FindAsync(providerData.Id);
        if (provider is null)
            return Result.Fail("Provider not found.");

        // Check if UserId is set
        if (providerData.UserId.HasValue)
            provider.UserId = providerData.UserId ?? provider.UserId;
        if (!string.IsNullOrWhiteSpace(providerData.CompanyName))
            provider.CompanyName = providerData.CompanyName;

        dbContext.Providers.Update(provider);
        await dbContext.SaveChangesAsync();

        return Result.Ok(provider);
    }
    
    public async Task<Result> DeleteProviderAsync(DeleteProviderData providerData)
    {
        var validation = await ValidateAsync(deleteProviderValidator, providerData);
        if (validation.IsFailed)
            return validation;

        var provider = await dbContext.Providers.FindAsync(providerData.Id);
        if (provider is null)
            return Result.Fail("Provider not found.");

        dbContext.Providers.Remove(provider);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}