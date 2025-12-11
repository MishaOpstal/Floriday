using System.Security.Claims;
using LeafBidAPI.DTOs.User;

namespace LeafBidAPI.Interfaces;

public interface IUserService
{
    Task<List<UserResponse>> GetUsers();
    Task<UserResponse> GetUserById(string id);
    Task<UserResponse> RegisterUser(CreateUserDto userData);
    Task<UserResponse> UpdateUser(string id, UpdateUserDto updatedUser);
    Task<UserResponse> UpdateUser(ClaimsPrincipal user, UpdateUserDto updatedUser);
    Task<UserResponse> LoginUser(LoginUserDto loginData);
    Task<bool> LogoutUser(ClaimsPrincipal user);
    Task<LoggedInUserResponse> GetLoggedInUser(ClaimsPrincipal user);
    Task<bool> DeleteUser(string id);
}