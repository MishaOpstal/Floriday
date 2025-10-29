using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeafBidAPI.App.Domain.User.Data;
using LeafBidAPI.App.Domain.User.Enums;
using LeafBidAPI.App.Domain.User.Repositories;
using LeafBidAPI.App.Infrastructure.Common.Data;
using LeafBidAPI.App.Infrastructure.Common.Http.Controllers;
using LeafBidAPI.App.Interfaces.User.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.App.Domain.User.Http.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController(
    ApplicationDbContext context,
    UserRepository userRepository,
    IMapper mapper
) : BaseController(context)
{
    /// <summary>
    /// Get all users.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<UserResource>>> GetUsers()
    {
        var users = await Context.Users
            .AsNoTracking()
            .ProjectTo<UserResource>(mapper.ConfigurationProvider)
            .ToListAsync();

        return new JsonResult(users) { StatusCode = 200 };
    }

    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserResource>> GetUser(int id)
    {
        var result = await userRepository.GetUserAsync(new GetUserData(id));

        if (result.IsFailed)
            return NotFound();

        var resource = mapper.Map<UserResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Create a new user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserResource>> CreateUser([FromBody] CreateUserRequest request)
    {
        var result = await userRepository.CreateUserAsync(
            new CreateUserData(request.Name, request.Email, request.Password, request.UserType)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<UserResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 201 };
    }

    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserResource>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var result = await userRepository.UpdateUserAsync(
            new UpdateUserData(id, request.Name, request.Email, request.Password, request.UserType)
        );

        if (result.IsFailed)
            return BadRequest(result.Errors);

        var resource = mapper.Map<UserResource>(result.Value);
        return new JsonResult(resource) { StatusCode = 200 };
    }

    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var result = await userRepository.DeleteUserAsync(new DeleteUserData(id));

        return result.IsFailed ? BadRequest(result.Errors) : new OkResult();
    }
}

public record CreateUserRequest(string Name, string Email, string Password, UserTypeEnum UserType);
public record UpdateUserRequest(string? Name, string? Email, string? Password, UserTypeEnum? UserType);