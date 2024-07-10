using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Application.Erros;
using BooKing.Apartments.Application.Interfaces;
using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Generics.Domain;

namespace BooKing.Apartments.Application.Services;
public class AmenityService : IAmenityService
{
    private readonly IAmenityRepository _amenityRepository;

    public AmenityService(IAmenityRepository amenityRepository)
    {
        _amenityRepository = amenityRepository;
    }

    public async Task<Result<Amenity>> CreateAsync(NewAmenityDto dto)
    {
        var newAmenity = new Amenity(dto.Name);

        await _amenityRepository.AddAsync(newAmenity);

        return Result.Success(newAmenity);
    }

    public async Task<Result> Delete(Guid id)
    {
        var amenity = await _amenityRepository.GetByIdAsync(id);
        if (amenity is null)
            return Result.Failure(ApplicationErrors.AmenityError.ProvidedAmenityNotFound);

        _amenityRepository.Delete(amenity);

        return Result.Success();
    }

    public async Task<Result<List<Amenity>>> GetAll()
    {
        var amenitites = await _amenityRepository.GetAllAsync();

        return Result.Success(amenitites);
    }
}
