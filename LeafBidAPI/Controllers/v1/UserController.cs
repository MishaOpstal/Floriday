using System.Security.Claims;
using LeafBidAPI.Data;
using LeafBidAPI.DTOs.User;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    ApplicationDbContext context,
    SignInManager<User> signInManager,
    UserManager<User> userManager)
    : BaseController(context)
{
    /// <summary>
    /// Get all users.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return await Context.Users.ToListAsync();
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        User? user = await Context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult> RegisterUser(CreateUserDto userData)
    {
        try
        {
            User user = new()
            {
                UserName = userData.UserName,
                Email = userData.Email,
            };
            
            // Check if passwords match
            if (userData.Password != userData.PasswordConfirmation)
            {
                return BadRequest("Passwords do not match.");
            }

            IdentityResult result = await userManager.CreateAsync(user, userData.Password);
            
            // If roles are provided assign them
            if (userData.Roles != null && !Array.Empty<string>().Equals(userData.Roles))
            {
                result = await userManager.AddToRolesAsync(user, userData.Roles);
            }

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong, please try again. " + ex.Message);
        }

        return OkResult("Registered Successfully.");
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto login)
    {
        var user = await userManager.FindByEmailAsync(login.Email ?? "");
        if (user == null) return Unauthorized("Invalid credentials.");

        var result = await signInManager.PasswordSignInAsync(
            user,
            login.Password,
            isPersistent: login.Remember,
            lockoutOnFailure: false
        );

        if (!result.Succeeded) return Unauthorized("Invalid credentials.");

        user.LastLogin = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        // Optional: return the same shape as /me so your frontend can hydrate immediately
        var roles = await userManager.GetRolesAsync(user);
        return Ok(new
        {
            loggedIn = true,
            userData = new
            {
                user.LastLogin,
                user.Id,
                user.UserName,
                user.NormalizedUserName,
                user.Email,
                user.NormalizedEmail,
                user.EmailConfirmed,
                LockoutEnd = user.LockoutEnd?.UtcDateTime,
                user.LockoutEnabled,
                user.AccessFailedCount,
                Roles = roles
            }
        });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> LogoutUser()
    {
        try
        {
            await signInManager.SignOutAsync();
        }
        catch (Exception ex)
        {
            return BadRequest("Something went wrong, please try again. " + ex.Message);
        }

        return OkResult("You are free to go!");
    }

    /// <summary>
    /// Retrieve current user data
    /// </summary>
    [HttpGet("me")]
    [AllowAnonymous]
    public async Task<ActionResult> LoggedInUser()
    {
        // Get the currently authorized user
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized(new GetLoggedInUserDto
            {
                LoggedIn = false,
                UserData = null
            });
        }

        var roles = await userManager.GetRolesAsync(user);

        var dto = new GetLoggedInUserDto
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

        return new JsonResult(dto);
    }

    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult> UpdateUser(
        string id,
        [FromBody] UpdateUserDto updatedUser
    )
    {
        User? user = await userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(updatedUser.UserName))
        {
            IdentityResult updateUserNameResult = await userManager.SetUserNameAsync(user, updatedUser.UserName);

            if (!updateUserNameResult.Succeeded)
            {
                return BadRequest(updateUserNameResult.Errors);
            }
        }

        if (!string.IsNullOrEmpty(updatedUser.Email))
        {
            IdentityResult updateUserEmailResult = await userManager.SetEmailAsync(user, updatedUser.Email);

            if (!updateUserEmailResult.Succeeded)
            {
                return BadRequest(updateUserEmailResult.Errors);
            }
        }

        if (!string.IsNullOrEmpty(updatedUser.Password))
        {
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult resetResult = await userManager.ResetPasswordAsync(user, token, updatedUser.Password);

            if (!resetResult.Succeeded)
            {
                return BadRequest(resetResult.Errors);
            }
        }
        
        await userManager.UpdateAsync(user);

        return OkResult("User updated successfully");
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        ActionResult<User> user = await GetUser(id);
        if (user.Value == null)
        {
            return NotFound();
        }

        Context.Users.Remove(user.Value);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}