namespace LeafBidAPI.DTOs.Product;

public class UpdateProductDto
{
    /// <summary>
    /// Data required to update a product
    /// </summary>

    public required string Name { get; set; }
    public required string Description { get; set; }
    public required decimal MinPrice { get; set; }
    public required double Weight { get; set; }
    public required string Picture { get; set; }
    public required string Species { get; set; }
    public required string Region { get; set; }
    public double? PotSize { get; set; }
    public double? StemLength { get; set; }
    public required int Stock { get; set; }
    public required DateTime HarvestedAt { get; set; }
}