using BooKing.Apartments.Application.Dtos;
using BooKing.Generics.Domain;
using BooKing.Generics.Shared;

namespace BooKing.Apartments.Application.Interfaces;
public interface IApartmentService
{
    Task<Result> CreateApartmentAsync(NewApartmentDto apartmentDto);
    Task<Result<ApartmentDto>> UpdateApartmentAsync(Guid id, UpdateApartmentDto apartmentDto);
    Task<Result> DeleteApartmentAsync(Guid id);
    Task<Result<ApartmentDto>> GetApartmentByIdAsync(Guid id);
    Task<Result<PaginatedList<ApartmentDto>>> GetPaginatedApartmentsAsync(int pageIndex, int pageSize);
    Task<Result<List<ApartmentDto>>> GetApartmentsByGuids(List<Guid> apartmentGuids);
    Task<Result<List<ApartmentDto>>> GetApartmentsByUserId(Guid userId);
    Task<Result<ApartmentDto>> PatchApartmentIsActive(Guid apartmentId, bool isActive);
    Task<Result> CountUserApartmentsCreated();
}
