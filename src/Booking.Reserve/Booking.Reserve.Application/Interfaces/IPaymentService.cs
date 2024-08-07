using BooKing.Reserve.Application.Dtos;

namespace BooKing.Reserve.Application.Interfaces;
public interface IPaymentService
{
    Task<PaymentResultDto> ProcessPaymentAsync(Guid reservationId, decimal totalPrice, bool setSuccessPayment = false);
}
