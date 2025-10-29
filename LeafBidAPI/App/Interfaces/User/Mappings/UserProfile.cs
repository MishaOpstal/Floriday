using AutoMapper;
using LeafBidAPI.App.Interfaces.User.Resources;

namespace LeafBidAPI.App.Interfaces.User.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Map User entity -> UserResource
            CreateMap<Domain.User.Models.User, UserResource>();
        }
    }
}