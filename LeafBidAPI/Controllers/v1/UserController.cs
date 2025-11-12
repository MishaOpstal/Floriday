using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext context) : BaseController(context)
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
        var user = await Context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }
    
    /// <summary>
    /// Create a new user.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        return new JsonResult(user) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, User updatedUser)
    {
        var user = await GetUser(id);
        if (user.Value == null)
        {
            return NotFound();
        }

        user.Value.Name = updatedUser.Name;
        user.Value.Email = updatedUser.Email;
        user.Value.PasswordHash = updatedUser.PasswordHash;
        user.Value.UserType = updatedUser.UserType;
        
        await Context.SaveChangesAsync();
        return new JsonResult(user.Value);
    }
    
    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await GetUser(id);
        if (user.Value == null)
        {
            return NotFound();
        }

        Context.Users.Remove(user.Value);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}