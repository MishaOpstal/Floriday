namespace LeafBidAPI.DTOs.Product;

public class UpdateProductDto
{
    /// <summary>
    /// Data required to update a product
    /// </summary>
    
    public required int Id { get; set; }
    public string? Name { get; set; }
    public double? Weight { get; set; }
    public string? Picture { get; set; }
    public string? Species { get; set; }
    public int? PotSize { get; set; }
    public int? StemLength { get; set; }
    public int? Stock { get; set; }
    public int? AuctionId { get; set; }
}