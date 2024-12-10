using BooKing.Reserve.Application.Dtos;
using BooKing.Generics.Domain;

namespace BooKing.Reserve.Application.Interfaces;
public interface IReservationService
{
    Task<Result<ReservationCreatedDto>> Reserve(NewReservationDto dto);
    Task<Result> ConfirmReserve(Guid reservationId);
    Task<Result> CancelReserve(Guid reservationId);    
    Task<Result<List<ReservationDto>>> GetAllReservationsByUserId(Guid userId);
    Task<Result<List<ReservationEventsDto>>> GetReservationEvents(Guid reservationId);
    Task<Result<ReservationDto>> GetReservation(Guid reservationId);
    Task<Result<int>> CountUserReservations();
}
