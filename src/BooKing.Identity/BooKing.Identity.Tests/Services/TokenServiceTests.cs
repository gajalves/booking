using BooKing.Generics.Api.Extensions;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Application.Services;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace BooKing.Identity.Tests.Services;
public class TokenServiceTests
{
    private readonly ITokenService _sut;

    public TokenServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "supersecretkeythatshouldbelonger",
            Issuer = "TokenServiceTestsIssuer",
            Audience = "TokenServiceTestsAudience",
            ExpiresInHours = 1
        };
        var options = Options.Create(jwtSettings);
        _sut = new TokenService(options);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidToken()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";

        // Act
        var response = _sut.GenerateToken(userId, email);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(response.AccessToken));
        Assert.True(response.ExpiresIn > 0);
    }

    [Fact]
    public void GenerateToken_ShouldContainCorrectClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";

        // Act
        var response = _sut.GenerateToken(userId, email);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(response.AccessToken);

        // Assert
        Assert.Equal(userId.ToString(), jwtToken.Claims.First(claim => claim.Type == "nameid").Value);
        Assert.Equal(email, jwtToken.Claims.First(claim => claim.Type == "email").Value);
    }

    [Fact]
    public void GenerateToken_ShouldExpireInConfiguredHours()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var email = "test@example.com";
        var expectedExpiration = DateTime.UtcNow.AddHours(1);

        // Act
        var response = _sut.GenerateToken(userId, email);
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(response.AccessToken);

        // Assert
        Assert.True(jwtToken.ValidTo <= expectedExpiration);
    }
}
