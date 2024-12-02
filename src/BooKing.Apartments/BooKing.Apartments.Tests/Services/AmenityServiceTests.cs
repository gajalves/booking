using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Application.Erros;
using BooKing.Apartments.Application.Services;
using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using Moq;

namespace BooKing.Apartments.Tests.Services;

public class AmenityServiceTests
{
    private readonly Mock<IAmenityRepository> _mockAmenityRepository;
    private readonly AmenityService _sut;

    public AmenityServiceTests()
    {
        _mockAmenityRepository = new Mock<IAmenityRepository>();
        _sut = new AmenityService(_mockAmenityRepository.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnSuccessResult_WhenAmenityIsCreated()
    {
        // Arrange
        var newAmenityDto = new NewAmenityDto { Name = "WiFi" };
        var createdAmenity = new Amenity(newAmenityDto.Name);

        _mockAmenityRepository.Setup(x => x.AddAsync(It.IsAny<Amenity>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateAsync(newAmenityDto);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newAmenityDto.Name, result.Value.Name);
        _mockAmenityRepository.Verify(x => x.AddAsync(It.IsAny<Amenity>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnSuccessResult_WhenAmenityExists()
    {
        // Arrange
        var amenityId = Guid.NewGuid();
        var existingAmenity = new Amenity("WiFi") { Id = amenityId };

        _mockAmenityRepository.Setup(x => x.GetByIdAsync(amenityId)).ReturnsAsync(existingAmenity);
        _mockAmenityRepository.Setup(x => x.Delete(existingAmenity)).Verifiable();

        // Act
        var result = await _sut.Delete(amenityId);

        // Assert
        Assert.True(result.IsSuccess);
        _mockAmenityRepository.Verify(x => x.Delete(existingAmenity), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldReturnFailureResult_WhenAmenityDoesNotExist()
    {
        // Arrange
        var amenityId = Guid.NewGuid();
        _mockAmenityRepository.Setup(x => x.GetByIdAsync(amenityId)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.Delete(amenityId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.AmenityError.ProvidedAmenityNotFound, result.Error);
        _mockAmenityRepository.Verify(x => x.Delete(It.IsAny<Amenity>()), Times.Never);
    }

    [Fact]
    public async Task GetAll_ShouldReturnListOfAmenities_WhenAmenitiesExist()
    {
        // Arrange
        var amenities = new List<Amenity>
        {
            new Amenity("WiFi"),
            new Amenity("Pool"),
            new Amenity("Gym")
        };

        _mockAmenityRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(amenities);

        // Act
        var result = await _sut.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(amenities.Count, result.Value.Count);
        _mockAmenityRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoAmenitiesExist()
    {
        // Arrange
        var emptyAmenities = new List<Amenity>();
        _mockAmenityRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(emptyAmenities);

        // Act
        var result = await _sut.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
        _mockAmenityRepository.Verify(x => x.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailureResult_WhenNameIsEmpty()
    {
        // Arrange
        var newAmenityDto = new NewAmenityDto { Name = "" };

        // Act
        var result = await _sut.CreateAsync(newAmenityDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.AmenityError.InvalidAmenityName, result.Error);
        _mockAmenityRepository.Verify(x => x.AddAsync(It.IsAny<Amenity>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnFailureResult_WhenNameIsNull()
    {
        // Arrange
        var newAmenityDto = new NewAmenityDto { Name = null };

        // Act
        var result = await _sut.CreateAsync(newAmenityDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.AmenityError.InvalidAmenityName, result.Error);
        _mockAmenityRepository.Verify(x => x.AddAsync(It.IsAny<Amenity>()), Times.Never);
    }
}
