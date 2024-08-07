using BooKing.Identity.Application.Dtos;

namespace BooKing.Identity.Application.Interfaces;
public interface ITokenService
{
    ResponseGenerateTokenDto GenerateToken(Guid id, string email);
}
