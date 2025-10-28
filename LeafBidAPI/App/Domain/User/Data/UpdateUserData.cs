using LeafBidAPI.Enums;

namespace LeafBidAPI.App.Domain.User.Data;

/// <summary>
/// Data used to update an existing user.
/// </summary>
public record UpdateUserData(
    int Id,
    string? Name = null,
    string? Email = null,
    string? Password = null,
    UserTypeEnum? UserType = null
);