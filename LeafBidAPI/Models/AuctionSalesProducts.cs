using System.Text.Json.Serialization;

namespace LeafBidAPI.Models;

/// <summary>
/// Represents the association between auction sales and products.
/// </summary>
public class AuctionSalesProducts
{
    /// <summary>
    /// Unique identifier for the auction sales product entry.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Identifier of the auction sale associated with the auction sale product
    /// </summary>
    public required int AuctionSaleId { get; set; }
    
    [JsonIgnore]
    public AuctionSales? AuctionSale { get; set; }
    
    /// <summary>
    /// Identifier of the product associated with the auction sale product
    /// </summary>
    public required int ProductId { get; set; }
    
    [JsonIgnore]
    public Product? Product { get; set; }
    
    /// <summary>
    /// Quantity of the product in the auction sale.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Price of the product in the auction sale.
    /// </summary>
    public int Price { get; set; }
}