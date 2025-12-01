using LeafBidAPI.Enums;
using System.Text.Json.Serialization;

namespace LeafBidAPI.DTOs.Auction;

public class UpdateAuctionDto
{
    public required DateTime StartTime {get; set;}
    public required ClockLocationEnum ClockLocationEnum {get; set;}
    
    public required string UserId {get; set;}
    
    [JsonIgnore]
    public required LeafBidAPI.Models.User User {get; set;} // doing it like this instead of a neat using up top, cus it throws an error without an explanation
}