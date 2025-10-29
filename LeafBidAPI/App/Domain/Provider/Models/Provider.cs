using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LeafBidAPI.App.Domain.Provider.Models;

/// <summary>
/// Represents a provider in the system.
/// </summary>
public class Provider
{
    /// <summary>
    /// Unique identifier for the provider.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identifier of the user associated with the provider
    /// </summary>
    public required int UserId { get; set; }
    
    [JsonIgnore]
    public User.Models.User? User { get; set; }
    
    /// <summary>
    /// Name of the provider's company.
    /// </summary>
    [MaxLength(255)]
    public required string CompanyName { get; set; }
}