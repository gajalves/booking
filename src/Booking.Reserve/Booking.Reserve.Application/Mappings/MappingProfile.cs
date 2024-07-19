using AutoMapper;
using Booking.Reserve.Application.Dtos;
using Booking.Reserve.Domain.Entities;

namespace Booking.Reserve.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reservation, ReservationCreatedDto>();
    }
}
