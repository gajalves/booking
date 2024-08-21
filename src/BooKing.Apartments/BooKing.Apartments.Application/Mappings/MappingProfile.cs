using AutoMapper;
using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.ValueObjects;

namespace BooKing.Apartments.Application.Mappings;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Apartment, ApartmentDto>()
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => src.Amenities));

        CreateMap<Amenity, AmenityDto>();
        CreateMap<Address, AddressDto>();
    }
}
