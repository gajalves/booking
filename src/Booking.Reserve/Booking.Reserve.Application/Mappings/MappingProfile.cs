using AutoMapper;
using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Domain.Entities;

namespace BooKing.Reserve.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reservation, ReservationCreatedDto>();
    }
}
