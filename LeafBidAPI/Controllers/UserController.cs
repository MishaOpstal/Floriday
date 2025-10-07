using LeafBidAPI.Data;
using LeafBidAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeafBidAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext dbContext) : BaseController(dbContext)
{
    /// <summary>
    /// Get all users.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        return await DbContext.Users.ToListAsync();
    }
    
    /// <summary>
    /// Get a user by ID.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await DbContext.Users.FindAsync(id);
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
        DbContext.Users.Add(user);
        await DbContext.SaveChangesAsync();

        return new JsonResult(user) { StatusCode = 201 };
    }
    
    /// <summary>
    /// Update an existing user by ID.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, User updatedUser)
    {
        var user = await DbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.PasswordHash = updatedUser.PasswordHash;
        user.UserType = updatedUser.UserType;
        
        await DbContext.SaveChangesAsync();
        return new JsonResult(user);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await DbContext.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        DbContext.Users.Remove(user);
        await DbContext.SaveChangesAsync();
        return new OkResult();
    }
}