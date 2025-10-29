using AutoMapper;
using LeafBidAPI.App.Interfaces.Auction.Resources;

namespace LeafBidAPI.App.Interfaces.Auction.Mappings
{
    public class AuctionProfile : Profile
    {
        public AuctionProfile()
        {
            // Map Auction entity -> AuctionResource
            CreateMap<Domain.Auction.Models.Auction, AuctionResource>();
        }
    }
}