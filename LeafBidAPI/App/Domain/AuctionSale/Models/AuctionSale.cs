using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LeafBidAPI.App.Domain.AuctionSale.Models;

/// <summary>
/// Represents a sale made at an auction.
/// </summary>
public class AuctionSale
{
    /// <summary>
    /// Unique identifier for the auction sale.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identifier for the auction where the sale took place.
    /// </summary>
    public required int AuctionId { get; set; }
    
    [JsonIgnore]
    public Auction.Models.Auction? Auction { get; set; }
    
    /// <summary>
    /// Unique identifier for the buyer who made the purchase.
    /// </summary>
    public required int BuyerId { get; set; }
    
    [JsonIgnore]
    public Buyer.Models.Buyer? Buyer { get; set; }
    
    /// <summary>
    /// Date and time when the sale occurred.
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Payment reference associated with the sale.
    /// </summary>
    [MaxLength(255)]
    public required string PaymentReference { get; set; }
}