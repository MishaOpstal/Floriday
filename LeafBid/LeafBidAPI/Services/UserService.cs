using System.Security.Claims;
using LeafBidAPI.Data;
using LeafBidAPI.DTOs.User;
using LeafBidAPI.Exceptions;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Services;

public class UserService(
    ApplicationDbContext context,
    SignInManager<User> signInManager,
    UserManager<User> userManager) : IUserService
{

    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserResponse>> GetUsers()
    {
        List<User> users = await context.Users.ToListAsync();

        List<UserResponse> userResponses = users.Select(user => new UserResponse
        {
            LastLogin = user.LastLogin,
            Id = user.Id,
            AccessFailedCount = user.AccessFailedCount,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            NormalizedEmail = user.NormalizedEmail ?? "",
            NormalizedUserName = user.NormalizedUserName ?? "",
            Roles = userManager.GetRolesAsync(user).Result
        }).ToList();

        return userResponses;
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserResponse> GetUserById(string id)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        UserResponse userResponse = new()
        {
            LastLogin = user.LastLogin,
            Id = user.Id,
            AccessFailedCount = user.AccessFailedCount,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            NormalizedEmail = user.NormalizedEmail ?? "",
            NormalizedUserName = user.NormalizedUserName ?? "",
            Roles = userManager.GetRolesAsync(user).Result
        };

        return userResponse;
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    /// <exception cref="PasswordMismatchException"></exception>
    /// <exception cref="UserCreationFailedException"></exception>
    public async Task<UserResponse> RegisterUser(CreateUserDto userData)
    {
        User user = new()
        {
            UserName = userData.UserName,
            Email = userData.Email,
        };

        // Check if passwords match
        if (userData.Password != userData.PasswordConfirmation)
        {
            throw new PasswordMismatchException("Passwords do not match");
        }

        IdentityResult result = await userManager.CreateAsync(user, userData.Password);

        // If roles are provided assign them
        if (userData.Roles != null && !Array.Empty<string>().Equals(userData.Roles))
        {
            result = await userManager.AddToRolesAsync(user, userData.Roles);
        }

        if (!result.Succeeded)
        {
            throw new UserCreationFailedException("User creation failed");
        }

        UserResponse userResponse = new()
        {
            LastLogin = user.LastLogin,
            Id = user.Id,
            AccessFailedCount = user.AccessFailedCount,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            NormalizedEmail = user.NormalizedEmail ?? "",
            NormalizedUserName = user.NormalizedUserName ?? "",
            Roles = await userManager.GetRolesAsync(user)
        };

        return userResponse;
    }

    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="UserUpdateFailedException"></exception>
    public async Task<UserResponse> UpdateUser(string id, UpdateUserDto updatedUser)
    {
        User? user = context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        if (!string.IsNullOrEmpty(updatedUser.UserName))
        {
            IdentityResult updateUserNameResult = await userManager.SetUserNameAsync(user, updatedUser.UserName);

            if (!updateUserNameResult.Succeeded)
            {
                throw new UserUpdateFailedException("Failed to update username");
            }
        }

        if (!string.IsNullOrEmpty(updatedUser.Email))
        {
            IdentityResult updateEmailResult = await userManager.SetEmailAsync(user, updatedUser.Email);

            if (!updateEmailResult.Succeeded)
            {
                throw new UserUpdateFailedException("Failed to update email");
            }
        }

        if (!string.IsNullOrEmpty(updatedUser.Password))
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult resetResult = await userManager.ResetPasswordAsync(user, token, updatedUser.Password);

            if (!resetResult.Succeeded)
            {
                throw new UserUpdateFailedException("Failed to update password");
            }
        }

        UserResponse userResponse = new()
        {
            LastLogin = user.LastLogin,
            Id = user.Id,
            AccessFailedCount = user.AccessFailedCount,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            NormalizedEmail = user.NormalizedEmail ?? "",
            NormalizedUserName = user.NormalizedUserName ?? "",
            Roles = await userManager.GetRolesAsync(user)
        };

        return userResponse;
    }

    /// <summary>
    /// Proxy to update the logged in user
    /// </summary>
    /// <param name="loggedInUser"></param>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<UserResponse> UpdateUser(ClaimsPrincipal loggedInUser, UpdateUserDto updatedUser)
    {
        User? user = await userManager.GetUserAsync(loggedInUser);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        return await UpdateUser(user.Id, updatedUser);
    }

    /// <summary>
    /// Login a user with email and password.
    /// </summary>
    /// <param name="loginData"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="UnauthorizedException"></exception>
    public async Task<UserResponse> LoginUser(LoginUserDto loginData)
    {
        User? user = await userManager.FindByEmailAsync(loginData.Email);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        SignInResult result = await signInManager.PasswordSignInAsync(
            user,
            loginData.Password,
            loginData.Remember,
            false
        );

        if (!result.Succeeded)
        {
            throw new UnauthorizedException("Invalid credentials");
        }

        user.LastLogin = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        UserResponse userResponse = new()
        {
            LastLogin = user.LastLogin,
            Id = user.Id,
            AccessFailedCount = user.AccessFailedCount,
            UserName = user.UserName ?? "",
            Email = user.Email ?? "",
            EmailConfirmed = user.EmailConfirmed,
            LockoutEnabled = user.LockoutEnabled,
            LockoutEnd = user.LockoutEnd,
            NormalizedEmail = user.NormalizedEmail ?? "",
            NormalizedUserName = user.NormalizedUserName ?? "",
            Roles = await userManager.GetRolesAsync(user)
        };

        return userResponse;
    }

    /// <summary>
    /// Logout the currently logged-in user.
    /// </summary>
    /// <param name="loggedInUser"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<bool> LogoutUser(ClaimsPrincipal loggedInUser)
    {
        User? user = await userManager.GetUserAsync(loggedInUser);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        await signInManager.SignOutAsync();
        return true;
    }

    /// <summary>
    /// Retrieve current user data
    /// </summary>
    /// <param name="loggedInUser"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task<LoggedInUserResponse> GetLoggedInUser(ClaimsPrincipal loggedInUser)
    {
        User? user = await userManager.GetUserAsync(loggedInUser);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        IList<string> roles = await userManager.GetRolesAsync(user);

        LoggedInUserResponse loggedInUserResponse = new()
        {
            LoggedIn = true,
            UserData = new UserResponse
            {
                LastLogin = user.LastLogin ?? DateTime.MinValue,
                Id = user.Id,
                UserName = user.UserName ?? "",
                NormalizedUserName = user.NormalizedUserName ?? "",
                Email = user.Email ?? "",
                NormalizedEmail = user.NormalizedEmail ?? "",
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnd = user.LockoutEnd?.UtcDateTime,
                LockoutEnabled = user.LockoutEnabled,
                AccessFailedCount = user.AccessFailedCount,
                Roles = roles
            }
        };

        return loggedInUserResponse;
    }

    public async Task<bool> DeleteUser(string id)
    {
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        IdentityResult result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}