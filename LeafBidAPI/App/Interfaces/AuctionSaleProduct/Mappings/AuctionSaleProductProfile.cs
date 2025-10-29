using AutoMapper;
using LeafBidAPI.App.Interfaces.AuctionSaleProduct.Resources;

namespace LeafBidAPI.App.Interfaces.AuctionSaleProduct.Mappings
{
    public class AuctionSaleProductProfile : Profile
    {
        public AuctionSaleProductProfile()
        {
            // Map AuctionSaleProduct entity -> AuctionSaleProductResource
            CreateMap<Domain.AuctionSaleProduct.Models.AuctionSaleProduct, AuctionSaleProductResource>();
        }
    }
}