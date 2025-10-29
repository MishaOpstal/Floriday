using LeafBidAPI.Enums;

namespace LeafBidAPI.App.Interfaces.User.Resources;

/// <summary>
/// Resource representing a user returned by the API.
/// </summary>
public record UserResource(
    int Id,
    string Name,
    string Email,
    UserTypeEnum UserType
);