using BooKing.Generics.Api.Extensions;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BooKing.Identity.Application.Services;
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;

    public TokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public ReturnLoginUserDto GenerateToken(Guid id, string email)
    {
        string encodedToken = WriteToken(id, email);

        return GenerateResponse(encodedToken, id ,email);
    }

    private ReturnLoginUserDto GenerateResponse(string encodedToken, Guid id, string email)
    {
        return new ReturnLoginUserDto
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(_jwtSettings.ExpiresInHours).TotalSeconds,
            UserId = id,
            UserEmail = email
        };
    }

    private string WriteToken(Guid id, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, email)
            }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiresInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
