using AutoMapper;
using LeafBidAPI.App.Interfaces.Provider.Resources;

namespace LeafBidAPI.App.Interfaces.Provider.Mappings
{
    public class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            // Map Provider entity -> ProviderResource
            CreateMap<Domain.Provider.Models.Provider, ProviderResource>();
        }
    }
}