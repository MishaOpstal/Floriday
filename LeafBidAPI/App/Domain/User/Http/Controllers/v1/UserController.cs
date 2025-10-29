using LeafBidAPI.App.Domain.User.Data;
using LeafBidAPI.App.Domain.User.Repositories;
using LeafBidAPI.Controllers;
using LeafBidAPI.Data;
using LeafBidAPI.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.User.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext context, UserRepository userRepository) : BaseController(context)
{

    /// <summary>
    /// Get all users.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<Models.User>>> GetUsers()
    {
        return await Context.Users.ToListAsync();
    }
    
    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.User>> GetUser(int id)
    {
        var user = await userRepository.GetUserAsync(
            new GetUserData(id)
        );
        
        return user.IsFailed ? NotFound() : new JsonResult(user.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Create a new user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Models.User>> CreateUser([FromBody] CreateUserRequest request)
    {
        var user = await userRepository.CreateUserAsync(
            new CreateUserData(request.Name, request.Email, request.Password, request.UserType)
        );
        
        return user.IsFailed ? BadRequest(user.Errors) : new JsonResult(user.Value) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Models.User>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var user = await userRepository.UpdateUserAsync(
            new UpdateUserData(id, request.Name, request.Email, request.Password, request.UserType)
        );
        
        return user.IsFailed ? BadRequest(user.Errors) : new JsonResult(user.Value) { StatusCode = 200 };
    }
    
    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await userRepository.DeleteUserAsync(
            new DeleteUserData(id)
        );
        
        return result.IsFailed ? BadRequest(result.Errors) : new OkResult();
    }
}

public record CreateUserRequest(string Name, string Email, string Password, UserTypeEnum UserType);
public record UpdateUserRequest(string? Name, string? Email, string? Password, UserTypeEnum? UserType);