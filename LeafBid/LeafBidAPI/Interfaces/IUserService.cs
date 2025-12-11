using System.Security.Claims;
using LeafBidAPI.DTOs.User;
using LeafBidAPI.Models;

namespace LeafBidAPI.Interfaces;

public interface IUserService
{
    Task<List<User>> GetUsers();
    Task<User> GetUserById(string id);
    Task<User> RegisterUser(CreateUserDto userData);
    Task<User> UpdateUser(string id, UpdateUserDto updatedUser);
    Task<User> UpdateUser(ClaimsPrincipal user, UpdateUserDto updatedUser);
    Task<User> LoginUser(LoginUserDto loginData);
    Task<bool> LogoutUser(ClaimsPrincipal user);
    Task<LoggedInUserResponse> GetLoggedInUser(ClaimsPrincipal user);
    Task<bool> DeleteUser(string id);
    UserResponse CreateUserResponse(User user, IList<string> roles);
}