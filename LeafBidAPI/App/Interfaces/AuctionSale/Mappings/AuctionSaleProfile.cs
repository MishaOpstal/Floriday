using AutoMapper;
using LeafBidAPI.App.Interfaces.AuctionSale.Resources;

namespace LeafBidAPI.App.Interfaces.AuctionSale.Mappings
{
    public class AuctionSaleProfile : Profile
    {
        public AuctionSaleProfile()
        {
            // Map Auction entity -> AuctionResource
            CreateMap<Domain.AuctionSale.Models.AuctionSale, AuctionSaleResource>();
        }
    }
}