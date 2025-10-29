using FluentResults;
using LeafBidAPI.App.Domain.User.Data;
using LeafBidAPI.App.Domain.User.Validators;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Repositories;
using LeafBidAPI.App.Infrastructure.User.Services;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.User.Repositories;

public class UserRepository(
    ApplicationDbContext dbContext,
    CreateUserValidator createUserValidator,
    GetUserValidator getUserValidator,
    UpdateUserValidator updateUserValidator,
    DeleteUserValidator deleteUserValidator,
    PasswordHasher passwordHasher) : BaseRepository
{
    public async Task<Result<Models.User>> GetUserAsync(GetUserData userData)
    {
        var validation = await ValidateAsync(getUserValidator, userData);
        if (validation.IsFailed)
            return validation.ToResult<Models.User>();

        var user = await dbContext.Users.FindAsync(userData.Id);
        return user is null
            ? Result.Fail("User not found.")
            : Result.Ok(user);
    }
    
    public async Task<Result<Models.User>> CreateUserAsync(CreateUserData userData)
    {
        var validation = await ValidateAsync(createUserValidator, userData);
        if (validation.IsFailed)
            return validation.ToResult<Models.User>();

        // Prevent duplicate emails
        bool exists = await dbContext.Users.AnyAsync(u => u.Email == userData.Email);
        if (exists)
            return Result.Fail("Email is already in use.");

        var user = new Models.User
        {
            Name = userData.Name,
            Email = userData.Email,
            PasswordHash = passwordHasher.Hash(userData.Password),
            UserType = userData.UserType
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return Result.Ok(user);
    }
    
    public async Task<Result<Models.User>> UpdateUserAsync(UpdateUserData userData)
    {
        var validation = await ValidateAsync(updateUserValidator, userData);
        if (validation.IsFailed)
            return validation.ToResult<Models.User>();

        var user = await dbContext.Users.FindAsync(userData.Id);
        if (user is null)
            return Result.Fail("User not found.");

        if (!string.IsNullOrWhiteSpace(userData.Name))
            user.Name = userData.Name;
        if (!string.IsNullOrWhiteSpace(userData.Email))
            user.Email = userData.Email;
        if (!string.IsNullOrWhiteSpace(userData.Password))
            user.PasswordHash = passwordHasher.Hash(userData.Password);
        if (userData.UserType.HasValue)
            user.UserType = userData.UserType.Value;

        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();

        return Result.Ok(user);
    }
    
    public async Task<Result> DeleteUserAsync(DeleteUserData userData)
    {
        var validation = await ValidateAsync(deleteUserValidator, userData);
        if (validation.IsFailed)
            return validation;

        var user = await dbContext.Users.FindAsync(userData.Id);
        if (user is null)
            return Result.Fail("User not found.");

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();

        return Result.Ok();
    }
}