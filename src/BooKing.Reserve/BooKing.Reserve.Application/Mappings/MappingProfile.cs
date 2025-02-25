using AutoMapper;
using BooKing.Generics.Shared;
using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Domain.Entities;

namespace BooKing.Reserve.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reservation, ReservationCreatedDto>();

        CreateMap<Reservation, ReservationDto>()
            .ForMember(dto => dto.StatusDescription, m => m.MapFrom(r => r.Status.GetEnumDescription()))
            .ForMember(dto => dto.StatusValue, m => m.MapFrom(r => (int)r.Status));
    }
}
