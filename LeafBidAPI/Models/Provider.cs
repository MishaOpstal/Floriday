namespace LeafBidAPI.Models;

/// <summary>
/// Represents a provider in the system.
/// </summary>
public class Provider
{
    /// <summary>
    /// Unique identifier for the provider.
    /// </summary>
    public int Id { get; set; }
    
    public User User { get; set; }
    
    /// <summary>
    /// Name of the provider's company.
    /// </summary>
    public string CompanyName { get; set; }
}