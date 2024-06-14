using BooKing.Generics.Domain;
using BooKing.Identity.Application.Dtos;

namespace BooKing.Identity.Application.Interfaces;
public interface IUserRegisterService
{
    Task<Result<ReturnCreatedUserDto>> Register(UserRegisterDto dto);
}
