using BooKing.Reserve.Application.Dtos;
using BooKing.Generics.Domain;

namespace BooKing.Reserve.Application.Interfaces;
public interface IReservationService
{
    Task<Result> Reserve(NewReservationDto dto);
    Task<Result> ConfirmReserve(Guid reservationId);
    Task<Result> CancelReserve(Guid reservationId);    
    Task<Result> GetAllReservationsByUserId(Guid userId);
    Task<Result> GetReservationEvents(Guid reservationId);
    Task<Result> GetReservation(Guid reservationId);
    Task<Result> CountUserReservations();
}
