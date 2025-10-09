﻿using LeafBidAPI.Data;
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
        var user = await Context.Users.FindAsync(id);
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
        var user = await Context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.PasswordHash = updatedUser.PasswordHash;
        user.UserType = updatedUser.UserType;
        
        await Context.SaveChangesAsync();
        return new JsonResult(user);
    }
    
    /// <summary>
    /// Delete a user by ID.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var user = await Context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        Context.Users.Remove(user);
        await Context.SaveChangesAsync();
        return new OkResult();
    }
}