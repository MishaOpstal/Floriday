namespace LeafBidAPI.Models;

public class Provider
{
    public int Id { get; set; }
    public User User { get; set; }
    public string CompanyName { get; set; }
}