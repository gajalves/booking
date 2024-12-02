using BooKing.Identity.Application.Interfaces;
using BooKing.Identity.Application.Services;

namespace BooKing.Identity.Tests.Services;
public class PasswordServiceTests
{
    private readonly IPasswordService _sut;

    public PasswordServiceTests()
    {
        _sut = new PasswordService();
    }

    [Fact]
    public void GenerateSalt_ShouldReturnNonEmptyString()
    {
        // Act
        var salt = _sut.GenerateSalt();

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(salt));
    }

    [Theory]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(64)]
    public void GenerateSalt_WithDifferentSizes_ShouldReturnCorrectLength(int size)
    {
        // Act
        var salt = _sut.GenerateSalt(size);

        // Assert
        var saltBytes = Convert.FromBase64String(salt);
        Assert.Equal(size, saltBytes.Length);
    }

    [Fact]
    public void HashPassword_ShouldReturnNonEmptyString()
    {
        // Arrange
        var password = "password123";
        var salt = _sut.GenerateSalt();

        // Act
        var hashedPassword = _sut.HashPassword(password, salt);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(hashedPassword));
    }

    [Fact]
    public void HashPassword_WithSameInputs_ShouldReturnSameHash()
    {
        // Arrange
        var password = "password123";
        var salt = _sut.GenerateSalt();
        var hash1 = _sut.HashPassword(password, salt);
        var hash2 = _sut.HashPassword(password, salt);

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void HashPassword_WithDifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password1 = "password123";
        var password2 = "differentPassword";
        var salt = _sut.GenerateSalt();
        var hash1 = _sut.HashPassword(password1, salt);
        var hash2 = _sut.HashPassword(password2, salt);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void HashPassword_WithSameInputs_AndDifferentSalt_ShouldReturnDifferentHashes()
    {
        // Arrange
        var password = "password123";
        var salt1 = _sut.GenerateSalt();
        var salt2 = _sut.GenerateSalt();
        var hash1 = _sut.HashPassword(password, salt1);
        var hash2 = _sut.HashPassword(password, salt2);

        // Assert
        Assert.NotEqual(hash1, hash2);
    }
}