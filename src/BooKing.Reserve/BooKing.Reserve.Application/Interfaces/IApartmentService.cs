using BooKing.Reserve.Application.Dtos;
using BooKing.Generics.Domain;

namespace BooKing.Reserve.Application.Interfaces;
public interface IApartmentService
{
    Task<Result<GetApartmentDto>> GetApartment(Guid apartmentId);

    Task<Result<List<GetApartmentDto>>> GetApartmentByGuidList(List<Guid> apartmentIds);
}
