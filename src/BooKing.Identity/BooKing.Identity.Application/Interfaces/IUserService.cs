using BooKing.Generics.Domain;
using BooKing.Identity.Application.Dtos;

namespace BooKing.Identity.Application.Interfaces;
public interface IUserService
{
    Task<Result<UserInformationDto>> GetUserInformation(Guid userId);
}
