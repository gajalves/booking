using AutoMapper;
using BooKing.Generics.EventSourcing;
using BooKing.Generics.Outbox.Service;
using BooKing.Identity.Application;
using BooKing.Identity.Application.Dtos;
using BooKing.Identity.Application.Erros;
using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Application.Services;
using BooKing.Identity.Domain.Entities;
using BooKing.Identity.Domain.Interfaces;
using Moq;

namespace BooKing.Identity.Tests.Services;
public class UserRegisterServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IOutboxEventService> _outboxEventServiceMock;
    private readonly IMapper _mapper;

    private readonly IUserRegisterService _sut;

    public UserRegisterServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _outboxEventServiceMock = new Mock<IOutboxEventService>();

        _mapper = AutoMapperConfiguration.Create().CreateMapper();

        _sut = new UserRegisterService(
            _userRepositoryMock.Object,
            _passwordServiceMock.Object,
            _mapper,
            _outboxEventServiceMock.Object);
    }

    [Fact]    
    public async Task Register_EmailAlreadyInUse_ReturnsFailure()
    {
        // Arrange
        var dto = new UserRegisterDto { Email = "test@example.com", Name = "Test User", Password = "password123" };
        _userRepositoryMock.Setup(x => x.EmailAlreadyInUse(dto.Email)).ReturnsAsync(true);

        // Act
        var result = await _sut.Register(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.UserError.EmailAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task Register_UserCreationFails_ReturnsFailure()
    {
        // Arrange
        var dto = new UserRegisterDto { Email = "test@example.com", Name = "Test User", Password = "password123" };
        _userRepositoryMock.Setup(x => x.EmailAlreadyInUse(dto.Email)).ReturnsAsync(false);
        _passwordServiceMock.Setup(x => x.GenerateSalt(32)).Returns("salt");
        _passwordServiceMock.Setup(x => x.HashPassword(dto.Password, "salt", 100000)).Returns("hashedpassword");
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(() => null);

        // Act
        var result = await _sut.Register(dto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.UserError.UnexpectedErrorCreatingUser, result.Error);
    }

    [Fact]
    public async Task Register_SuccessfulRegistration_ReturnsSuccess()
    {
        // Arrange
        var dto = new UserRegisterDto { Email = "test@example.com", Name = "Test User", Password = "password123" };
        var user = new User(dto.Email, dto.Name, "hashedpassword", "salt") { Id = Guid.NewGuid() };
        _userRepositoryMock.Setup(x => x.EmailAlreadyInUse(dto.Email)).ReturnsAsync(false);
        _passwordServiceMock.Setup(x => x.GenerateSalt(32)).Returns("salt");
        _passwordServiceMock.Setup(x => x.HashPassword(dto.Password, "salt", 100000)).Returns("hashedpassword");
        _userRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(user);
        _outboxEventServiceMock.Setup(service => service.AddEvent(It.IsAny<string>(), It.IsAny<Event>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.Register(dto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(user.Email, result.Value.Email);
        Assert.Equal(user.Name, result.Value.Name);
    }    
}
