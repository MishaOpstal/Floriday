using System.Text.Json.Serialization;

namespace LeafBidAPI.App.Domain.AuctionSaleProduct.Models;

/// <summary>
/// Represents the association between auction sales and products.
/// </summary>
public class AuctionSaleProduct
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
    public AuctionSale.Models.AuctionSale? AuctionSale { get; set; }
    
    /// <summary>
    /// Identifier of the product associated with the auction sale product
    /// </summary>
    public required int ProductId { get; set; }
    
    [JsonIgnore]
    public Product.Models.Product? Product { get; set; }
    
    /// <summary>
    /// Quantity of the product in the auction sale.
    /// </summary>
    public int Quantity { get; set; }
    
    /// <summary>
    /// Price of the product in the auction sale.
    /// </summary>
    public decimal Price { get; set; }
}