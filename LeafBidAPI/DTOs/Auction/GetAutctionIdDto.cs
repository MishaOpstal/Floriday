namespace LeafBidAPI.DTOs.Auction;

public class GetAutctionIdDto
{
    /// <summary>
    /// Data returned when requesting auction information by ID
    /// </summary>
    public required int Id { get; set; }
}