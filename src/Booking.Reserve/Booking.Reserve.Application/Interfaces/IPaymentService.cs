using Booking.Reserve.Application.Dtos;

namespace Booking.Reserve.Application.Interfaces;
public interface IPaymentService
{
    Task<PaymentResultDto> ProcessPaymentAsync(Guid reservationId, decimal totalPrice, bool setSuccessPayment = false);
}
