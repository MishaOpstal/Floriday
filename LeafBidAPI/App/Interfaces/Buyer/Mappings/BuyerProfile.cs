using AutoMapper;
using LeafBidAPI.App.Interfaces.Buyer.Resources;

namespace LeafBidAPI.App.Interfaces.Buyer.Mappings
{
    public class BuyerProfile : Profile
    {
        public BuyerProfile()
        {
            // Map Buyer entity -> BuyerResource
            CreateMap<Domain.Buyer.Models.Buyer, BuyerResource>();
        }
    }
}