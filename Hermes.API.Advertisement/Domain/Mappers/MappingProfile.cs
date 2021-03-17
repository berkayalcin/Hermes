using AutoMapper;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Advertisement, AdvertisementDto>().ReverseMap();
            CreateMap<CreateAdvertisementRequest, Entities.Advertisement>();
            CreateMap<Entities.Category, CategoryDto>().ReverseMap();
            CreateMap<Entities.User, UserDto>().ReverseMap();
        }
    }
}