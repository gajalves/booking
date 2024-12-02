using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Identity.Application.Erros;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Application.Services;
using BooKing.Identity.Domain.Entities;
using BooKing.Identity.Domain.Interfaces;
using Moq;

namespace BooKing.Identity.Tests.Services;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly IUserService _sut;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _sut = new UserService(_userRepositoryMock.Object, _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task GetUserInformation_UserIdDoesNotMatchCurrentUser_ReturnsNotAllowedError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser(Guid.NewGuid(), "test@example.com", "abc123");
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        // Act
        var result = await _sut.GetUserInformation(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.UserError.NotAllowedToRetrieveThisInformation, result.Error);
    }

    [Fact]
    public async Task GetUserInformation_UserNotFound_ReturnsUserDoesNotExistError()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser(userId, "test@example.com", "abc123");
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(currentUser.Email)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.GetUserInformation(userId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.UserError.UserDoesNotExists, result.Error);
    }

    [Fact]
    public async Task GetUserInformation_Successful_ReturnsUserInformationDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser(userId, "test@example.com", "abc123");
        var user = new User("test@example.com", "Test User", "hashedpassword", "salt");
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(currentUser.Email)).ReturnsAsync(user);

        // Act
        var result = await _sut.GetUserInformation(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(currentUser.Email, result.Value.Email);
        Assert.Equal(user.Name, result.Value.UserName);
    }
}
