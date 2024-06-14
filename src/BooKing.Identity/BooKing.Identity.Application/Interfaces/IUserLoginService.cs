using BooKing.Generics.Domain;
using BooKing.Identity.Application.Dtos;

namespace BooKing.Identity.Application.Interfaces;
public interface IUserLoginService
{
    Task<Result<ReturnLoginUserDto>> Login(UserLoginDto dto);
}
