namespace LeafBidAPI.App.Domain.User.Exceptions;

public class UserNotFoundException(int userId) : Exception($"User with ID {userId} was not found.");