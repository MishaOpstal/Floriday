using AutoMapper;
using LeafBidAPI.App.Interfaces.Auctioneer.Resources;

namespace LeafBidAPI.App.Interfaces.Auctioneer.Mappings
{
    public class AuctioneerProfile : Profile
    {
        public AuctioneerProfile()
        {
            // Map Auctioneer entity -> AuctioneerResource
            CreateMap<Domain.Auctioneer.Models.Auctioneer, AuctioneerResource>();
        }
    }
}