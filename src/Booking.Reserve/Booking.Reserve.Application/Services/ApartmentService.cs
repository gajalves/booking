using Booking.Reserve.Application.Dtos;
using Booking.Reserve.Application.Interfaces;
using BooKing.Generics.Domain;
using BooKing.Generics.Shared;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Generics.Shared.HttpClientService;

namespace Booking.Reserve.Application.Services;
public class ApartmentService : IApartmentService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ICurrentUserService _currentUserService;
    private readonly string _baseUrl;

    public ApartmentService(IHttpClientService httpClientService,
                            ICurrentUserService currentUserService,
                            ExternalServices externalServices)
    {
        _httpClientService = httpClientService;
        _baseUrl = externalServices.ApartmentServiceUrl;
        _currentUserService = currentUserService;
    }

    public async Task<Result<GetApartmentDto>> GetApartment(Guid apartmentId)
    {
        var user = _currentUserService.GetCurrentUser();

        _httpClientService.SetBearerToken(user.Token);

        var endpoint = $"{_baseUrl}/apartment/{apartmentId}";
        var result = await _httpClientService.GetAsync<Result<GetApartmentDto>>(_baseUrl, endpoint);
        
        return result;
    }
}
