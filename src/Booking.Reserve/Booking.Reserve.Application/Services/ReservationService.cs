using AutoMapper;
using Azure.Core;
using Booking.Reserve.Application.Dtos;
using Booking.Reserve.Application.Erros;
using Booking.Reserve.Application.Exceptions;
using Booking.Reserve.Application.Interfaces;
using Booking.Reserve.Domain;
using Booking.Reserve.Domain.Entities;
using Booking.Reserve.Domain.Interfaces;
using Booking.Reserve.Domain.ValueObjects;
using BooKing.Generics.Domain;
using BooKing.Generics.Shared.CurrentUserService;

namespace Booking.Reserve.Application.Services;
public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IApartmentService _apartmentService;
    private readonly PricingService _pricingService;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository reservationRepository,
                              ICurrentUserService currentUserService,
                              IApartmentService apartmentService,
                              PricingService pricingService,
                              IMapper mapper)
    {
        _reservationRepository = reservationRepository;
        _currentUserService = currentUserService;
        _apartmentService = apartmentService;
        _pricingService = pricingService;
        _mapper = mapper;
    }

    public async Task<Result> Reserve(NewReservationDto dto)
    {
        var result = await _apartmentService.GetApartment(dto.ApartmentId);
        
        if (result.IsFailure)
            return result;

        var apartment = result.Value;

        var user = _currentUserService.GetCurrentUser();

        if (user.Id.ToString() == apartment.OwnerId)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToReserveYourOwnApartment);
        
        var duration = DateRange.Create(dto.StartDate, dto.EndDate);

        var isApartmentOverlapping = await _reservationRepository.IsOverlappingAsync(dto.ApartmentId, duration);
        
        if (isApartmentOverlapping)
            return Result.Failure<Guid>(ApplicationErrors.ReserveError.Overlap);

        try
        {
            var reserve = Reservation.Reserve(
                apartment.Id,
                user.Id,
                apartment.Price,
                apartment.CleaningFee,
                duration,
                _pricingService);

            await _reservationRepository.AddAsync(reserve);

            return Result.Success(reserve);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(ApplicationErrors.ReserveError.Overlap);
        }
    }
    public Task CancelReserve(CancelReserveDto dto)
    {
        throw new NotImplementedException();
    }
}
