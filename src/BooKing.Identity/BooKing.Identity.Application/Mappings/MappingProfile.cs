using AutoMapper;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Domain.Entities;

namespace BooKing.Identity.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, ReturnCreatedUserDto>().ReverseMap();
    }
}
