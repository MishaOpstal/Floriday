using LeafBidAPI.DTOs.User;
using LeafBidAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeafBidAPI.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
// [Authorize]
[AllowAnonymous]
[ApiVersion("2.0")]
[Produces("application/json")]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Get all users.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<UserResponse>>> GetUsers()
    {
        List<UserResponse> users = await userService.GetUsers();
        return Ok(users);
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> GetUser(string id)
    {
        UserResponse user = await userService.GetUserById(id);
        return Ok(user);
    }

    /// <summary>
    /// Register a new user.
    /// </summary>
    /// <param name="userData"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<UserResponse>> RegisterUser([FromBody] CreateUserDto userData)
    {
        UserResponse created = await userService.RegisterUser(userData);

        return CreatedAtAction(
            actionName: nameof(GetUser),
            routeValues: new { id = created.Id, version = "2.0" },
            value: created
        );
    }

    /// <summary>
    /// Login a user.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> LoginUser([FromBody] LoginUserDto login)
    {
        UserResponse user = await userService.LoginUser(login);
        return Ok(user);
    }

    /// <summary>
    /// Logout the current user.
    /// </summary>
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// <param name="id"></param>
    /// <param name="updatedUser"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<UserResponse>> UpdateUser(string id, [FromBody] UpdateUserDto updatedUser)
    {
        UserResponse updated = await userService.UpdateUser(id, updatedUser);
        return Ok(updated);
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteUser(string id)
    {
        bool ok = await userService.DeleteUser(id);
        return ok ? NoContent() : Problem("Delete failed.");
    }
}
