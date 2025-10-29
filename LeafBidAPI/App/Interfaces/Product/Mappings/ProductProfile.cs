using AutoMapper;
using LeafBidAPI.App.Interfaces.Product.Resources;

namespace LeafBidAPI.App.Interfaces.Product.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Map Product entity -> ProductResource
            CreateMap<Domain.Product.Models.Product, ProductResource>();
        }
    }
}