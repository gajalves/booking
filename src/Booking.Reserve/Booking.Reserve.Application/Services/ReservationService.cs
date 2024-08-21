using AutoMapper;
using BooKing.Generics.Bus.Queues;
using BooKing.Generics.Domain;
using BooKing.Generics.EventSourcing;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Application.Erros;
using BooKing.Reserve.Application.Exceptions;
using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Domain;
using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Domain.ValueObjects;

namespace BooKing.Reserve.Application.Services;
public class ReservationService : IReservationService
{
    private readonly IReservationRepository _reservationRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IApartmentService _apartmentService;
    private readonly PricingService _pricingService;
    private readonly IOutboxEventService _outboxEventService;
    private readonly IEventSourcingRepository _eventSourcingRepository;
    private readonly IMapper _mapper;

    public ReservationService(IReservationRepository reservationRepository,
                              ICurrentUserService currentUserService,
                              IApartmentService apartmentService,
                              PricingService pricingService,
                              IOutboxEventService outboxEventService,
                              IMapper mapper,
                              IEventSourcingRepository eventSourcingRepository)
    {
        _reservationRepository = reservationRepository;
        _currentUserService = currentUserService;
        _apartmentService = apartmentService;
        _pricingService = pricingService;
        _outboxEventService = outboxEventService;
        _mapper = mapper;
        _eventSourcingRepository = eventSourcingRepository;
    }

    public async Task<Result> Reserve(NewReservationDto dto)
    {
        var result = await _apartmentService.GetApartment(dto.ApartmentId);

        if (result.IsFailure)
            return result;

        var apartment = result.Value;

        var user = _currentUserService.GetCurrentUser();

        if (user.Id.ToString().ToLower() == apartment.OwnerId.ToLower())
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

            await _outboxEventService.AddEvent(
                QueueMapping.BooKingEmailServiceReservationCreated,
                new ReservationCreatedEvent(reserve.Id, user.Id, user.Email, apartment.Id, 
                                            apartment.Name, reserve.Duration.Start, reserve.Duration.End, 
                                            reserve.PriceForPeriod, reserve.CleaningFee, 
                                            reserve.TotalPrice, reserve.CreatedOnUtc));

            var reservationDto = _mapper.Map<ReservationCreatedDto>(reserve);

            return Result.Success(reservationDto);
        }
        catch (ConcurrencyException)
        {
            return Result.Failure<Guid>(ApplicationErrors.ReserveError.Overlap);
        }
    }

    public async Task<Result> ConfirmReserve(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return Result.Failure(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToConfirmReservation);

        if (reservation.Status != ReservationStatus.Pending)
            return Result.Failure(ApplicationErrors.ReserveError.InvalidStatus);

        reservation.Confirm();

        _reservationRepository.Update(reservation);

        var ev = new ReservationConfirmedByUserEvent(reservation.Id, user.Id, user.Email);
        await _outboxEventService.AddEvent(QueueMapping.BooKingReserveReservationConfirmed, ev);

        return Result.Success();
    }

    public async Task<Result> CancelReserve(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return Result.Failure(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToConfirmReservation);

        if (reservation.Status != ReservationStatus.Pending)
            return Result.Failure(ApplicationErrors.ReserveError.InvalidStatus);

        reservation.Cancel();

        _reservationRepository.Update(reservation);

        var ev = new ReservationCancelledByUserEvent(reservation.Id, user.Id, user.Email, 
                                                     reservation.Duration.Start, reservation.Duration.End, reservation.TotalPrice);
        await _outboxEventService.AddEvent(QueueMapping.BooKingReserveReservationCancelled, ev);

        return Result.Success();
    }

    public async Task<Result> GetAllReservationsByUserId(Guid userId)
    {
        var user = _currentUserService.GetCurrentUser();
        if (userId != user.Id)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation);

        var reservations = await _reservationRepository.GetAllReservationsByUserId(userId);

        var reservationsDtos = reservations.Select(r => _mapper.Map<ReservationDto>(r))
                                           .OrderByDescending(r => r.CreatedOnUtc)
                                           .ToList();

        if (reservations.Any())
        {
            var apartmentIds = reservations.Select(r => r.ApartmentId).ToList();
            var result = await _apartmentService.GetApartmentByGuidList(apartmentIds);

            if (result.IsFailure)
                return Result.Failure(ApplicationErrors.ReserveError.ErrorObtainingApartmentData);

            var apartments = result.Value;

            foreach ( var reservation in reservationsDtos) 
            {
                var apartment = apartments.Where(a => a.Id == reservation.ApartmentId).FirstOrDefault();
                if(apartment is not null)
                    reservation.Apartment = apartment;
            }
        }

        return Result.Success(reservationsDtos);
    }

    public async Task<Result> GetReservation(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetReservation(reservationId);

        if(reservation == null)
            return Result.Failure(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation);

        var result = await _apartmentService.GetApartment(reservation.ApartmentId);

        if (result.IsFailure)
            return Result.Failure(ApplicationErrors.ReserveError.ErrorObtainingApartmentData);

        var reservationDto = _mapper.Map<ReservationDto>(reservation);
        reservationDto.Apartment = result.Value;
        
        return Result.Success(reservationDto);
    }

    public async Task<Result> GetReservationEvents(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetReservation(reservationId);

        if (reservation == null)
            return Result.Failure(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation);

        var events = await _eventSourcingRepository.GetEvents(reservationId);

        if(events == null)
            return Result.Failure(ApplicationErrors.ReserveError.EventsNotFound);

        var reservationEventsDto = HandleReservationEvents(events);
        
        return Result.Success(reservationEventsDto);
    }

    private List<ReservationEventsDto> HandleReservationEvents(IEnumerable<StoredEvent> events)
    {
        var reservationEventsDto = events.Select(e => new ReservationEventsDto
        {
            Id = e.Id,
            EventType = e.EventType,
            EventTypeDescription = MapEventTypeToDescription(e.EventType),
            Icon = MapEventTypeToIcon(e.EventType),
            CreatedAt = e.CreatedAt
        })
        .OrderBy(e => e.CreatedAt)
        .ToList();

        return reservationEventsDto;
    }

    private string MapEventTypeToDescription(string eventType)
    {
        return eventType switch
        {
            "ReservationCreatedEvent" => "Reservation Created",
            "ReservationConfirmedByUserEvent" => "Reservation Confirmed",
            "ReservationCancelledByUserEvent" => "Reservation Cancelled",
            "ReservationPaymentInitiatedEvent" => "Payment Initiated",
            "ReservationPaymentProcessedEvent" => "Payment Processed",
            "ReservationReservedEvent" => "Reservation Finalized",
            "ReservationCompletedEvent" => "Reservation Completed",
            _ => "Unknown Event"
        };
    }

    private string MapEventTypeToIcon(string eventType)
    {
        return eventType switch
        {
            "ReservationCreatedEvent" => "bi bi-calendar2-check",
            "ReservationConfirmedByUserEvent" => "bi bi-person-check-fill",
            "ReservationCancelledByUserEvent" => "bi bi-person-x-fill",
            "ReservationPaymentInitiatedEvent" => "bi bi-credit-card",
            "ReservationPaymentProcessedEvent" => "bi bi-receipt",
            "ReservationReservedEvent" => "bi bi-check",
            "ReservationCompletedEvent" => "bi bi-hand-thumbs-up-fill",
            _ => "fa-question-circle",
        };
    }
}
