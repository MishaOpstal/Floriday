namespace LeafBidAPI.DTOs.Product;

public class CreateProductDto
{
    /// <summary>
    /// Data required to create a product
    /// </summary>
    
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string MinPrice { get; set; }
    public required string MaxPrice { get; set; }
    public required double Weight { get; set; }
    public required string Picture { get; set; }
    public required string Species { get; set; }
    public required string Region { get; set; }
    public required int PotSize { get; set; }
    public required int StemLength { get; set; }
    public required int Stock { get; set; }
    public required string HarvestedAt { get; set; }
    public required int ProviderId { get; set; }
    public required int AuctionId { get; set; }
}