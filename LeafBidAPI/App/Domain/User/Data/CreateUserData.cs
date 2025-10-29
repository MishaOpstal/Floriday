using LeafBidAPI.App.Domain.User.Enums;

namespace LeafBidAPI.App.Domain.User.Data;

/// <summary>
/// Data required to create a new user.
/// </summary>
public record CreateUserData(
    string Name,
    string Email,
    string Password, // Raw password — will be hashed inside your service layer
    UserTypeEnum UserType
);