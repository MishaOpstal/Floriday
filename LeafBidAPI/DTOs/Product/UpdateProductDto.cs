namespace LeafBidAPI.DTOs.Product;

public class UpdateProductDto
{
    /// <summary>
    /// Data required to update a product
    /// </summary>
    
    public required int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? MinPrice { get; set; }
    public string? MaxPrice { get; set; }
    public double? Weight { get; set; }
    public string? Picture { get; set; }
    public string? Species { get; set; }
    public string? Region { get; set; }
    public int? PotSize { get; set; }
    public int? StemLength { get; set; }
    public int? Stock { get; set; }
    public string? HarvestedAt { get; set; }
    public int? ProviderId { get; set; }
    public int? AuctionId { get; set; }
}