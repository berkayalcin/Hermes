using AutoMapper;
using Hermes.API.Catalog.Domain.Entities;
using Hermes.API.Catalog.Domain.Models;

namespace Hermes.API.Catalog.Domain.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}