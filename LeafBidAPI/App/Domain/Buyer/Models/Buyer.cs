using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LeafBidAPI.App.Domain.Buyer.Models;

/// <summary>
/// Represents a buyer in the system.
/// </summary>
public class Buyer
{
    /// <summary>
    /// Unique identifier for the buyer.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identifier of the user associated with the buyer
    /// </summary>
    public required int UserId { get; set; }
    
    [JsonIgnore]
    public User.Models.User? User { get; set; }
    
    /// <summary>
    /// Name of the buyer's company.
    /// </summary>
    [MaxLength(255)]
    public required string CompanyName { get; set; }
}