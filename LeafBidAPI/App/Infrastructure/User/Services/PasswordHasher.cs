namespace LeafBidAPI.App.Infrastructure.User.Services;

public class PasswordHasher
{
    public string Hash(string password)
    {
        // Hash with automatic salt generation (default work factor = 10)
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        // Compare the raw password with the stored hash
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}