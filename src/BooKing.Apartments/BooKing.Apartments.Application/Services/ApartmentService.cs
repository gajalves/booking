using AutoMapper;
using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Application.Erros;
using BooKing.Apartments.Application.Interfaces;
using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Apartments.Domain.ValueObjects;
using BooKing.Generics.Domain;
using BooKing.Generics.Shared;
using BooKing.Generics.Shared.CurrentUserService;
using Microsoft.AspNetCore.Http;

namespace BooKing.Apartments.Application.Services;
public class ApartmentService : IApartmentService
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUserService;

    public ApartmentService(IApartmentRepository apartmentRepository,
                            IAmenityRepository amenityRepository,
                            IMapper mapper,
                            ICurrentUserService currentUserService)
    {
        _apartmentRepository = apartmentRepository;
        _amenityRepository = amenityRepository;
        _mapper = mapper;        
        _currentUserService = currentUserService;
    }

    public async Task CreateApartmentAsync(NewApartmentDto dto)
    {
        var newApartment = await InstantiateNewApartment(dto);
        
        await _apartmentRepository.AddAsync(newApartment);
    }

    private async Task<Apartment> InstantiateNewApartment(NewApartmentDto dto)
    {
        var user = _currentUserService.GetCurrentUser();

        var address = new Address(
            dto.Address.Country,
            dto.Address.State,
            dto.Address.ZipCode,
            dto.Address.City,
            dto.Address.Street,
            dto.Address.Number);

        var newApartment = new Apartment(
            dto.Name,
            dto.Description,
            address,
            dto.Price,
            dto.CleaningFee,
            user.Id.ToString(),
            dto.Imagepath);

        foreach (var amenityId in dto.Amenities)
        {
            var amenity = await _amenityRepository.GetByIdAsync(amenityId);

            if (amenity == null)
                continue;

            newApartment.AddAmenitie(amenity);
        }

        return newApartment;
    }

    public async Task<Result> DeleteApartmentAsync(Guid id)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(id);

        if (apartment == null)
            return Result.Failure(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound);

        var user = GetUser();

        if (apartment.OwnerId != user.Id.ToString())
            return Result.Failure(ApplicationErrors.ApartmentError.NotAllowedToManageApartment);

        _apartmentRepository.Delete(apartment);

        return Result.Success("Apartment Deleted!");

    }

    public async Task<Result<ApartmentDto>> GetApartmentByIdAsync(Guid id)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(id);

        if (apartment == null)
            return Result.Failure<ApartmentDto>(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound);

        var mappedApartment = _mapper.Map<ApartmentDto>(apartment);
        
        return Result.Success<ApartmentDto>(mappedApartment);
    }

    public async Task<Result<PaginatedList<ApartmentDto>>> GetPaginatedApartmentsAsync(int pageIndex, int pageSize)
    {
        if (pageIndex <= 0 || pageSize <= 0)
            return Result.Failure<PaginatedList<ApartmentDto>>(ApplicationErrors.ApplicationError.PageIndexAndPageSizeMustBeGreaterThanZero);

        var apartments = await _apartmentRepository.ListPagedAsync(pageIndex, pageSize);

        var count = await _apartmentRepository.CountAsync();

        var apartmentDtos = apartments.Select(a => _mapper.Map<ApartmentDto>(a)).ToList();

        return Result.Success(new PaginatedList<ApartmentDto>(apartmentDtos, count, pageIndex, pageSize));
    }

    public async Task<Result<ApartmentDto>> UpdateApartmentAsync(Guid id, UpdateApartmentDto apartmentDto)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(id);
        if (apartment == null)
        {
            return Result.Failure<ApartmentDto>(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound);
        }

        var user = GetUser();

        if (apartment.OwnerId != user.Id.ToString())
            return Result.Failure<ApartmentDto>(ApplicationErrors.ApartmentError.NotAllowedToManageApartment);

        var address = new Address(
            apartmentDto.Address.Country,
            apartmentDto.Address.State,
            apartmentDto.Address.ZipCode,
            apartmentDto.Address.City,
            apartmentDto.Address.Street,
            apartmentDto.Address.Number);

        apartment.Update(
            apartmentDto.Name,
            apartmentDto.Description,
            address,
            apartmentDto.Price,
            apartmentDto.CleaningFee);

        var amenities = new List<Amenity>();
        foreach (var amenitieId in apartmentDto.Amenities)
        {
            var amenitie = await _amenityRepository.GetByIdAsync(amenitieId);
            if (amenitie != null)
            {
                amenities.Add(amenitie);
            }
        }

        apartment.SetAmenities(amenities);

        _apartmentRepository.Update(apartment);

        var mappedApartment = _mapper.Map<ApartmentDto>(apartment);

        return Result.Success(mappedApartment);
    }
    
    private CurrentUser GetUser()
    {
        return _currentUserService.GetCurrentUser();
    }

    public async Task<Result<List<ApartmentDto>>> GetApartmentsByGuids(List<Guid> apartmentGuids)
    {
        var apartments = await _apartmentRepository.GetApartmentsByGuids(apartmentGuids);

        var apartmentDtos = apartments.Select(a => _mapper.Map<ApartmentDto>(a)).ToList();

        return Result.Success<List<ApartmentDto>>(apartmentDtos);
    }
}
