using BooKing.Generics.Domain;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Erros;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Application.Services;
using BooKing.Identity.Domain.Entities;
using BooKing.Identity.Domain.Interfaces;
using Moq;

namespace BooKing.Identity.Tests.Services;
public class UserLoginServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    private readonly IUserLoginService _sut;

    public UserLoginServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _tokenServiceMock = new Mock<ITokenService>();

        _sut = new UserLoginService(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Login_EmailNotFound_ReturnsFailure()
    {
        // Arrange
        var dto = new UserLoginDto { Email = "test@example.com", Password = "password123" };
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.Login(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.UserError.ProvidedEmailAccountNotFound, result.Error);
    }

    [Fact]
    public async Task Login_PasswordIncorrect_ReturnsFailure()
    {
        // Arrange
        var dto = new UserLoginDto { Email = "test@example.com", Password = "wrongpassword" };
        var user = new User("test@example.com", "Test User", "hashedpassword", "salt");
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
        _passwordServiceMock.Setup(x => x.HashPassword(dto.Password, "salt", 100000)).Returns("incorrecthash");

        // Act
        var result = await _sut.Login(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.UserError.PasswordIncorrect, result.Error);
    }

    [Fact]
    public async Task Login_Successful_ReturnsSuccess()
    {
        // Arrange
        var dto = new UserLoginDto { Email = "test@example.com", Password = "correctpassword" };
        var user = new User("test@example.com", "Test User", "hashedpassword", "salt");
        var tokenResult = new ResponseGenerateTokenDto { AccessToken = "token", ExpiresIn = 1 };
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
        _passwordServiceMock.Setup(x => x.HashPassword(dto.Password, "salt", 100000)).Returns("hashedpassword");
        _tokenServiceMock.Setup(x => x.GenerateToken(user.Id, user.Email)).Returns(tokenResult);

        // Act
        var result = await _sut.Login(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(tokenResult.AccessToken, result.Value.AccessToken);
    }
}
