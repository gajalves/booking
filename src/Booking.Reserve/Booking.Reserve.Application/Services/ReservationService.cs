using AutoMapper;
using BooKing.Generics.Domain;
using BooKing.Generics.EventSourcing;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Application.Erros;
using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Domain;
using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;

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

    public async Task<Result<ReservationCreatedDto>> Reserve(NewReservationDto dto)
    {
        var result = await _apartmentService.GetApartment(dto.ApartmentId);
        if (result.IsFailure)
            return Result.Failure<ReservationCreatedDto>(result.Error);

        var apartment = result.Value;
        if (!apartment.IsActive)
            return Result.Failure<ReservationCreatedDto>(ApplicationErrors.ReserveError.ApartmentInactive);

        var user = _currentUserService.GetCurrentUser();

        if (user.Id.ToString().ToLower() == apartment.OwnerId.ToLower())
            return Result.Failure<ReservationCreatedDto>(ApplicationErrors.ReserveError.NotAllowedToReserveYourOwnApartment);

        var duration = DateRange.Create(dto.StartDate, dto.EndDate);
        if (duration.IsFailure)
            return Result.Failure<ReservationCreatedDto>(duration.Error);

        using var transaction = await _reservationRepository.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var isApartmentOverlapping = await _reservationRepository.IsOverlappingAsync(dto.ApartmentId, duration.Value, transaction);
            if (isApartmentOverlapping)
            {
                await transaction.RollbackAsync();
                return Result.Failure<ReservationCreatedDto>(ApplicationErrors.ReserveError.Overlap);
            }

            var reserve = Reservation.Reserve(
                apartment.Id,
                user.Id,
                apartment.Price,
                apartment.CleaningFee,
                duration.Value,
                _pricingService);

            await _reservationRepository.AddAsync(reserve);
            await _outboxEventService.AddEvent(
                new ReservationCreatedEvent(reserve.Id, user.Id, user.Email, apartment.Id,
                                            apartment.Name, reserve.Duration.Start, reserve.Duration.End,
                                            reserve.PriceForPeriod, reserve.CleaningFee,
                                            reserve.TotalPrice, reserve.CreatedOnUtc));

            await transaction.CommitAsync();

            var reservationDto = _mapper.Map<ReservationCreatedDto>(reserve);

            return Result.Success(reservationDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync();
            return Result.Failure<ReservationCreatedDto>(ApplicationErrors.ReserveError.Overlap);
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
        await _outboxEventService.AddEvent(ev);

        return Result.Success();
    }

    public async Task<Result> CancelReserve(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return Result.Failure(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure(ApplicationErrors.ReserveError.NotAllowedToCancelReservation);

        if (reservation.Status != ReservationStatus.Pending)
            return Result.Failure(ApplicationErrors.ReserveError.InvalidStatus);

        reservation.Cancel();

        _reservationRepository.Update(reservation);

        var ev = new ReservationCancelledByUserEvent(reservation.Id, user.Id, user.Email,
                                                     reservation.Duration.Start, reservation.Duration.End, reservation.TotalPrice);
        await _outboxEventService.AddEvent(ev);

        return Result.Success();
    }

    public async Task<Result<List<ReservationDto>>> GetAllReservationsByUserId(Guid userId)
    {
        var user = _currentUserService.GetCurrentUser();
        if (userId != user.Id)
            return Result.Failure<List<ReservationDto>>(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation);

        var reservations = await _reservationRepository.GetAllReservationsByUserId(userId);

        var reservationsDtos = reservations.Select(r => _mapper.Map<ReservationDto>(r))
                                           .OrderByDescending(r => r.CreatedOnUtc)
                                           .ToList();

        if (reservations.Any())
        {
            var apartmentIds = reservations.Select(r => r.ApartmentId).ToList();
            var result = await _apartmentService.GetApartmentByGuidList(apartmentIds);

            if (result.IsFailure)
                return Result.Failure<List<ReservationDto>>(ApplicationErrors.ReserveError.ErrorObtainingApartmentData);

            var apartments = result.Value;

            foreach (var reservation in reservationsDtos)
            {
                var apartment = apartments.Where(a => a.Id == reservation.ApartmentId).FirstOrDefault();
                if (apartment is not null)
                    reservation.Apartment = apartment;
            }
        }

        return Result.Success(reservationsDtos);
    }

    public async Task<Result<ReservationDto>> GetReservation(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetReservation(reservationId);

        if (reservation == null)
            return Result.Failure<ReservationDto>(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure<ReservationDto>(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation);

        var result = await _apartmentService.GetApartment(reservation.ApartmentId);

        if (result.IsFailure)
            return Result.Failure<ReservationDto>(ApplicationErrors.ReserveError.ErrorObtainingApartmentData);

        var reservationDto = _mapper.Map<ReservationDto>(reservation);
        reservationDto.Apartment = result.Value;

        return Result.Success(reservationDto);
    }

    public async Task<Result<List<ReservationEventsDto>>> GetReservationEvents(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetReservation(reservationId);

        if (reservation == null)
            return Result.Failure<List<ReservationEventsDto>>(ApplicationErrors.ReserveError.NotFound);

        var user = _currentUserService.GetCurrentUser();
        if (reservation.UserId != user.Id)
            return Result.Failure<List<ReservationEventsDto>>(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation);

        var events = await _eventSourcingRepository.GetEvents(reservationId);

        if (events == null)
            return Result.Failure<List<ReservationEventsDto>>(ApplicationErrors.ReserveError.EventsNotFound);

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
            CreatedAt = e.CreatedAt,
            AdditionalInformation = MapEventTypeToAdditionalInformation(e.EventType, e.Data),
        })
        .OrderBy(e => e.CreatedAt)
        .ToList();

        return reservationEventsDto;
    }

    private string MapEventTypeToAdditionalInformation(string eventType, string data)
    {
        return eventType switch
        {
            "ReservationCreatedEvent" => "",
            "ReservationConfirmedByUserEvent" => "Reservation confirmed by user",
            "ReservationCancelledByUserEvent" => "Reservation Cancelled by user",
            "ReservationPaymentInitiatedEvent" => "",
            "ReservationPaymentProcessedEvent" => GeneratePaymentProcessedEventInformation(data),
            "ReservationReservedEvent" => "Reservation process finalized",
            "ReservationCompletedEvent" => "Reservation Completed!",
            _ => "Unknown Event"
        };
    }

    private string GeneratePaymentProcessedEventInformation(string data)
    {
        if (string.IsNullOrEmpty(data))
            return string.Empty;

        var obj = JsonConvert.DeserializeObject<ReservationPaymentProcessedEvent>(data);

        return $"Payment status: {(obj.IsApproved ? "Approved" : "Rejected")}";
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

    public async Task<Result<int>> CountUserReservations()
    {
        var user = _currentUserService.GetCurrentUser();

        var count = await _reservationRepository.CountByUserIdAsync(user.Id);

        return Result.Success(count);
    }
}
