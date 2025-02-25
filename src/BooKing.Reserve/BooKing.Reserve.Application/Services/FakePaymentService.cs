using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Application.Interfaces;

namespace BooKing.Reserve.Application.Services;
public class FakePaymentService : IPaymentService
{
    private readonly Random _random;

    public FakePaymentService()
    {
        _random = new Random();
    }

    public Task<PaymentResultDto> ProcessPaymentAsync(Guid reservationId, decimal totalPrice, bool setSuccessPayment = false)
    {        
        bool isSuccess = _random.Next(0, 2) == 0; 

        if (isSuccess || setSuccessPayment)
        {
            return Task.FromResult(new PaymentResultDto
            {
                IsSuccess = true,
                Message = "Payment successful."
            });
        }
        else
        {
            return Task.FromResult(new PaymentResultDto
            {
                IsSuccess = false,
                Message = "Payment failed."
            });
        }
    }
}
