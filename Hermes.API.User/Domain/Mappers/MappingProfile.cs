using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using Hermes.API.User.Domain.Entities;
using Hermes.API.User.Domain.Requests;
using Hermes.API.User.Domain.Responses;

namespace Hermes.API.User.Domain.Mappers
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterRequest, HermesUser>()
                .ForMember(p => p.UserName,
                    t =>
                        t.MapFrom(src => src.Email));
            CreateMap<HermesUser, UserDto>();
            CreateMap<HermesRole, RoleDto>();
            CreateMap<CreateRoleRequest, HermesRole>();
            CreateMap<HermesUser, SearchUsersResponse>()
                .ForMember(dest => dest.Role,
                    opt =>
                        opt.MapFrom(src =>
                            src.UserRoles.FirstOrDefault().HermesRole.Name));
        }
    }
}