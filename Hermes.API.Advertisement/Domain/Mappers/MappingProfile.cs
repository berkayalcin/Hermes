using AutoMapper;
using Hermes.API.Advertisement.Domain.Entities;
using Hermes.API.Advertisement.Domain.Models;
using Hermes.API.Advertisement.Domain.Proxies.Models;
using Hermes.API.Advertisement.Domain.Requests;

namespace Hermes.API.Advertisement.Domain.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Advertisement, AdvertisementDto>().ReverseMap();
            CreateMap<CreateAdvertisementRequest, Entities.Advertisement>();
            CreateMap<UpdateAdvertisementRequest, Entities.Advertisement>();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<AdvertisementImage, AdvertisementImageDto>().ReverseMap();
            CreateMap<AdvertisementApplication, AdvertisementApplicationDto>().ReverseMap();
            CreateMap<UserReviewDto, UserReview>().ReverseMap();
            CreateMap<FavoriteDto, Favorite>().ReverseMap();
        }
    }
}