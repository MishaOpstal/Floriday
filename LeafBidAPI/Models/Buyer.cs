namespace LeafBidAPI.Models;

public class Buyer
{
    public int Id { get; set; }
    public User User { get; set; }
    public string CompanyName { get; set; }
}