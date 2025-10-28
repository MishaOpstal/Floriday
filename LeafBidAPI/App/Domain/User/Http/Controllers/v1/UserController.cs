using LeafBidAPI.App.Domain.User.Data;
using LeafBidAPI.App.Domain.User.Entities;
using LeafBidAPI.App.Domain.User.Repositories;
using LeafBidAPI.Data;
using LeafBidAPI.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext context, UserRepository userRepository) : BaseController(context)
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
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var result = await userRepository.GetUserAsync(new GetUserData(id));
        if (!result.IsFailed) return new JsonResult(result.Value);

        // Not found vs validation errors
        bool notFound = result.Errors.Any(e => e.Message.Contains("not found", StringComparison.OrdinalIgnoreCase));
        return notFound ? NotFound() : new BadRequestObjectResult(new { errors = result.Errors.Select(e => e.Message) });
    }
    
    /// <summary>
    /// Create a new user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
    {
        var data = new CreateUserData(request.Name, request.Email, request.Password, request.UserType);
        var result = await userRepository.CreateUserAsync(data);
        if (result.IsFailed)
        {
            return new BadRequestObjectResult(new { errors = result.Errors.Select(e => e.Message) });
        }

        var created = result.Value;
        return new JsonResult(created) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var data = new UpdateUserData(id, request.Name, request.Email, request.Password, request.UserType);
        var result = await userRepository.UpdateUserAsync(data);
        if (!result.IsFailed) return new JsonResult(result.Value);

        bool notFound = result.Errors.Any(e => e.Message.Contains("not found", StringComparison.OrdinalIgnoreCase));
        return notFound ? NotFound() : new BadRequestObjectResult(new { errors = result.Errors.Select(e => e.Message) });
    }
    
    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await userRepository.DeleteUserAsync(new DeleteUserData(id));
        if (!result.IsFailed) return new OkResult();

        bool notFound = result.Errors.Any(e => e.Message.Contains("not found", StringComparison.OrdinalIgnoreCase));
        return notFound ? NotFound() : new BadRequestObjectResult(new { errors = result.Errors.Select(e => e.Message) });
    }
}

public record CreateUserRequest(string Name, string Email, string Password, UserTypeEnum UserType);
public record UpdateUserRequest(string? Name, string? Email, string? Password, UserTypeEnum? UserType);