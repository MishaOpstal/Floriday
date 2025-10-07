namespace LeafBidAPI.Models;

/// <summary>
/// Represents a sale made at an auction.
/// </summary>
public class AuctionSales
{
    /// <summary>
    /// Unique identifier for the auction sale.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identifier for the auction where the sale took place.
    /// </summary>
    public int AuctionId { get; set; }
    
    public required Auction Auction { get; set; }
    
    /// <summary>
    /// Unique identifier for the buyer who made the purchase.
    /// </summary>
    public int BuyerId { get; set; }
    public required Buyer Buyer { get; set; }
    
    /// <summary>
    /// Date and time when the sale occurred.
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// Payment reference associated with the sale.
    /// </summary>
    public required string PaymentReference { get; set; }
}