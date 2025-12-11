using LeafBidAPI.DTOs.User;
using LeafBidAPI.Interfaces;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
// [Authorize]
[AllowAnonymous]
[Produces("application/json")]
public class UserController(IUserService userService, IRoleService roleService) : ControllerBase
{
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns>A list of all users.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        List<User> users = await userService.GetUsers();

        List<UserResponse> userResponses = new(users.Count);

        foreach (User user in users)
        {
            IList<string> roles = await roleService.GetRolesForUser(user);
            UserResponse response = userService.CreateUserResponse(user, roles);
            userResponses.Add(response);
        }

        return Ok(userResponses);
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>The requested user.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetUser(string id)
    {
        User user = await userService.GetUserById(id);
        UserResponse userResponse = userService.CreateUserResponse(
            user,
            await roleService.GetRolesForUser(user)
        );
        return Ok(userResponse);
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="userData">The user registration data.</param>
    /// <returns>The created user.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] CreateUserDto userData)
    {
        User createdUser = await userService.RegisterUser(userData);
        UserResponse createdUserResponse = userService.CreateUserResponse(
            createdUser,
            await roleService.GetRolesForUser(createdUser)
        );

        return CreatedAtAction(
            actionName: nameof(GetUser),
            routeValues: new { id = createdUserResponse.Id, version = "2.0" },
            value: createdUserResponse
        );
    }

    /// <summary>
    /// Login a user.
    /// </summary>
    /// <param name="login">The login credentials.</param>
    /// <returns>The logged-in user.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> LoginUser([FromBody] LoginUserDto login)
    {
        User user = await userService.LoginUser(login);
        UserResponse userResponse = userService.CreateUserResponse(
            user,
            await roleService.GetRolesForUser(user)
        );
        
        return Ok(userResponse);
    }

    /// <summary>
    /// Logout the current user.
    /// </summary>
    /// <returns>No content if logout succeeded.</returns>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> LogoutUser()
    {
        bool ok = await userService.LogoutUser(User);
        return ok ? NoContent() : Problem("Logout failed.");
    }

    /// <summary>
    /// Get the currently logged-in user.
    /// </summary>
    /// <returns>The currently logged-in user.</returns>
    [HttpGet("me")]
    [ProducesResponseType(typeof(LoggedInUserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoggedInUserResponse>> LoggedInUser()
    {
        LoggedInUserResponse me = await userService.GetLoggedInUser(User);
        return Ok(me);
    }

    /// <summary>
    /// Update a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="updatedUser">The updated user data.</param>
    /// <returns>The updated user.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> UpdateUser(
        string id,
        [FromBody] UpdateUserDto updatedUser)
    {
        User updated = await userService.UpdateUser(id, updatedUser);
        UserResponse userResponse = userService.CreateUserResponse(
            updated,
            await roleService.GetRolesForUser(updated)
        );
        
        return Ok(userResponse);
    }
    
    /// <summary>
    /// Update the current user.
    /// </summary>
    /// <param name="updatedUser">The updated user data.</param>
    /// <returns>The updated user.</returns>
    [HttpPut]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> UpdateUser ([FromBody] UpdateUserDto updatedUser)
    {
        User updated = await userService.UpdateUser(User, updatedUser);
        UserResponse userResponse = userService.CreateUserResponse(
            updated,
            await roleService.GetRolesForUser(updated)
        );
        
        return Ok(userResponse);
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <returns>No content if deletion succeeded.</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        bool ok = await userService.DeleteUser(id);
        return ok ? NoContent() : Problem("Delete failed.");
    }
}
