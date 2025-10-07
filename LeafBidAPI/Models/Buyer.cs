namespace LeafBidAPI.Models;

/// <summary>
/// Represents a buyer in the system.
/// </summary>
public class Buyer
{
    /// <summary>
    /// Unique identifier for the buyer.
    /// </summary>
    public int Id { get; set; }
    
    public User User { get; set; }
    
    /// <summary>
    /// Name of the buyer's company.
    /// </summary>
    public string CompanyName { get; set; }
}