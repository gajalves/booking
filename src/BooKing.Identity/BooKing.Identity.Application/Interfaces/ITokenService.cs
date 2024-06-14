using BooKing.Identity.Application.Dtos;

namespace BooKing.Identity.Application.Interfaces;
public interface ITokenService
{
    ReturnLoginUserDto GenerateToken(Guid id, string email);
}
