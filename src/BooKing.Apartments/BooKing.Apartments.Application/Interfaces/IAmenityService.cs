using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Domain.Entities;
using BooKing.Generics.Domain;

namespace BooKing.Apartments.Application.Interfaces;
public interface IAmenityService
{
    Task<Result<Amenity>> CreateAsync(NewAmenityDto dto);
    Task<Result> Delete(Guid id);
    Task<Result<List<Amenity>>> GetAll();
}
