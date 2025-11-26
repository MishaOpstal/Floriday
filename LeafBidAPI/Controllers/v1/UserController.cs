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
[Authorize]
[AllowAnonymous]
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
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return await Context.Users.ToListAsync();
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [HttpGet("{id}")]
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
    public async Task<ActionResult> RegisterUser(CreateUserDto userData)
    {
        try
        {
            User user = new()
            {
                UserName = userData.UserName,
                Email = userData.Email,
            };

            IdentityResult result = await userManager.CreateAsync(user, userData.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Something went wrong, please try again.");
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
    public async Task<IActionResult> LoginUser(LoginUserDto login)
    {
        User? user = await userManager.FindByEmailAsync(login.Email ?? "");
        if (user == null) return Unauthorized("Invalid credentials.");

        SignInResult check =
            await signInManager.CheckPasswordSignInAsync(user, login.Password ?? "", lockoutOnFailure: false);
        if (!check.Succeeded) return Unauthorized("Invalid credentials.");

        user.LastLogin = DateTime.UtcNow;
        await userManager.UpdateAsync(user);

        // Build the principal correctly (includes roles/claims)
        ClaimsPrincipal principal = await signInManager.CreateUserPrincipalAsync(user);

        // Tell SignInManager to use bearer scheme
        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        // Return SignInResult so the BearerToken handler produces the token response
        return SignIn(principal, IdentityConstants.BearerScheme);
    }

    [HttpGet("logout"), Authorize]
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

        return OkResult("User updated successfully");
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize]
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