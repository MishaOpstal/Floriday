namespace LeafBidAPI.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Weight { get; set; }
    public string Picture { get; set; }
    public string Species { get; set; }
    public double? PotSize { get; set; }
    public double? StemLength { get; set; }
    public int Stock { get; set; }
    public Auction Auction { get; set; }
}