using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Application.Interfaces;
using BooKing.Generics.Domain;
using BooKing.Generics.Shared;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Generics.Shared.HttpClientService;
using BooKing.Reserve.Application.Erros;

namespace BooKing.Reserve.Application.Services;
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

    public async Task<Result<List<GetApartmentDto>>> GetApartmentByGuidList(List<Guid> apartmentIds)
    {
        if (!apartmentIds.Any())
            return Result.Success(new List<GetApartmentDto>());

        var user = _currentUserService.GetCurrentUser();

        _httpClientService.SetBearerToken(user.Token);

        var endpoint = $"{_baseUrl}/apartment/GetApartmentsByGuids";
        var result = await _httpClientService.PostAsync<List<Guid>,Result<List<GetApartmentDto>>>(_baseUrl, endpoint, apartmentIds);

        return result;
    }
}
