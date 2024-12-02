using AutoMapper;
using BooKing.Apartments.Application;
using BooKing.Apartments.Application.Dtos;
using BooKing.Apartments.Application.Erros;
using BooKing.Apartments.Application.Services;
using BooKing.Apartments.Domain.Entities;
using BooKing.Apartments.Domain.Interfaces;
using BooKing.Apartments.Domain.ValueObjects;
using BooKing.Generics.Shared.CurrentUserService;
using Moq;

namespace BooKing.Apartments.Tests.Services;
public class ApartmentServiceTests
{
    private readonly Mock<IApartmentRepository> _apartmentRepositoryMock;
    private readonly Mock<IAmenityRepository> _amenityRepositoryMock;
    private readonly IMapper _mapper;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly ApartmentService _sut;

    public ApartmentServiceTests()
    {
        _apartmentRepositoryMock = new Mock<IApartmentRepository>();
        _amenityRepositoryMock = new Mock<IAmenityRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _mapper = AutoMapperConfiguration.Create().CreateMapper();

        _sut = new ApartmentService(
            _apartmentRepositoryMock.Object,
            _amenityRepositoryMock.Object,
            _mapper,
            _currentUserServiceMock.Object);
    }

    [Fact]
    public async Task CreateApartmentAsync_WithValidInput_ShouldReturnSuccessResult()
    {
        // Arrange
        var amenitieId = Guid.NewGuid();
        var amenities = new List<Guid> { amenitieId };
        var newApartmentDto = new NewApartmentDto
        {
            Name = "Test Apartment",
            Description = "Test Description",
            Price = 100,
            CleaningFee = 20,
            Imagepath = "test.jpg",
            Address = new AddressDto
            {
                Country = "Country",
                State = "State",
                ZipCode = "12345",
                City = "City",
                Street = "Street",
                Number = "10"
            },
            Amenities = amenities
        };
        var user = new CurrentUser(Guid.NewGuid(), "test@test.com", "abc123");
                               
        _currentUserServiceMock.Setup(s => s.GetCurrentUser()).Returns(user);
        _apartmentRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Apartment>())).Returns(Task.CompletedTask);
        _amenityRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Amenity(amenitieId, "Test Amenity"));

        // Act
        var result = await _sut.CreateApartmentAsync(newApartmentDto);

        // Assert
        Assert.True(result.IsSuccess);        
        _apartmentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Apartment>()), Times.Once);
    }

    [Fact]
    public async Task DeleteApartmentAsync_WithValidIdAndUser_ShouldReturnSuccessResult()
    {
        // Arrange
        var user = new CurrentUser(Guid.NewGuid(), "test@test.com", "abc123");
        var apartment = new Apartment("Test", "Description", new Address("Country", "State", "12345", "City", "Street", "10"), 100, 20, user.Id.ToString(), "test.jpg");

        _currentUserServiceMock.Setup(s => s.GetCurrentUser()).Returns(user);
        _apartmentRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(apartment);
        _apartmentRepositoryMock.Setup(r => r.Delete(It.IsAny<Apartment>())).Verifiable();

        // Act
        var result = await _sut.DeleteApartmentAsync(apartment.Id);

        // Assert
        Assert.True(result.IsSuccess);
        _apartmentRepositoryMock.Verify(r => r.Delete(It.IsAny<Apartment>()), Times.Once);
    }

    [Fact]
    public async Task DeleteApartmentAsync_WithInvalidId_ShouldReturnFailureResult()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();

        _apartmentRepositoryMock.Setup(r => r.GetByIdAsync(apartmentId)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.DeleteApartmentAsync(apartmentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound, result.Error);
        _apartmentRepositoryMock.Verify(r => r.Delete(It.IsAny<Apartment>()), Times.Never);
    }

    [Fact]
    public async Task GetApartmentByIdAsync_WithValidId_ShouldReturnSuccessResult()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        var apartment = new Apartment("Test", "Description", new Address("Country", "State", "12345", "City", "Street", "10"), 100, 20, "userId", "test.jpg");
        apartment.AddAmenitie(new Amenity(Guid.NewGuid(), "test"));
        var apartmentDto = _mapper.Map<ApartmentDto>(apartment);

        _apartmentRepositoryMock.Setup(r => r.GetByIdAsync(apartmentId)).ReturnsAsync(apartment);
        
        // Act
        var result = await _sut.GetApartmentByIdAsync(apartmentId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equivalent(apartmentDto, result.Value);
    }

    [Fact]
    public async Task GetApartmentByIdAsync_WithInvalidId_ShouldReturnFailureResult()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();

        _apartmentRepositoryMock.Setup(r => r.GetByIdAsync(apartmentId)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.GetApartmentByIdAsync(apartmentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound, result.Error);
    }

    [Fact]
    public async Task UpdateApartmentAsync_ShouldReturnSuccess_WhenApartmentIsUpdated()
    {
        // Arrange        
        var user = new CurrentUser(Guid.NewGuid(), "test@test.com", "abc123");
        var apartmentId = Guid.NewGuid();
        var apartment = new Apartment("Test Name", "Test Description", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, user.Id.ToString(), "image/path");
        var updateApartmentDto = new UpdateApartmentDto
        {
            Name = "Updated Name",
            Description = "Updated Description",
            Address = new AddressDto { Country = "New Country", State = "New State", ZipCode = "54321", City = "New City", Street = "New Street", Number = "321" },
            Price = 200,
            CleaningFee = 20,
            Imagepath = "new/image/path",
            Amenities = new List<Guid> { Guid.NewGuid() }
        };

        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(apartment);
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(user);
        _amenityRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Amenity("Amenity Name"));
        
        // Act
        var result = await _sut.UpdateApartmentAsync(apartmentId, updateApartmentDto);

        // Assert
        Assert.True(result.IsSuccess);
        _apartmentRepositoryMock.Verify(x => x.Update(apartment), Times.Once);
    }

    [Fact]
    public async Task UpdateApartmentAsync_ShouldReturnFailure_WhenApartmentNotFound()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        var updateApartmentDto = new UpdateApartmentDto();

        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.UpdateApartmentAsync(apartmentId, updateApartmentDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound, result.Error);
    }

    [Fact]
    public async Task GetPaginatedApartmentsAsync_ShouldReturnSuccess_WithPaginatedList()
    {
        // Arrange
        var pageIndex = 1;
        var pageSize = 2;
        var apartments = new List<Apartment>
        {
            new Apartment("Name1", "Description1", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, Guid.NewGuid().ToString(), "image/path1"),
            new Apartment("Name2", "Description2", new Address("Country", "State", "54321", "City", "Street", "321"), 200, 20, Guid.NewGuid().ToString(), "image/path2")
        };
        _apartmentRepositoryMock.Setup(x => x.ListPagedAsync(pageIndex, pageSize)).ReturnsAsync(apartments);
        _apartmentRepositoryMock.Setup(x => x.CountAsync()).ReturnsAsync(apartments.Count);
       
        // Act
        var result = await _sut.GetPaginatedApartmentsAsync(pageIndex, pageSize);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(pageSize, result.Value.Items.Count);
    }

    [Fact]
    public async Task GetPaginatedApartmentsAsync_ShouldReturnFailure_WhenPageIndexOrPageSizeIsZeroOrNegative()
    {
        // Arrange
        var pageIndex = 0;
        var pageSize = 0;

        // Act
        var result = await _sut.GetPaginatedApartmentsAsync(pageIndex, pageSize);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApplicationError.PageIndexAndPageSizeMustBeGreaterThanZero, result.Error);
    }

    [Fact]
    public async Task UpdateApartmentAsync_ShouldReturnFailure_WhenLoggedUserIsNotOwner()
    {
        // Arrange
        var apartmentOwnerId = Guid.NewGuid().ToString();
        var apartmentId = Guid.NewGuid();
        var apartment = new Apartment("Test Name", "Test Description",
            new Address("Country", "State", "12345", "City", "Street", "123"),
            100, 10, apartmentOwnerId, "image/path");

        var updateApartmentDto = new UpdateApartmentDto
        {
            Name = "Updated Name",
            Description = "Updated Description",
            Address = new AddressDto { Country = "New Country", State = "New State", ZipCode = "54321", City = "New City", Street = "New Street", Number = "321" },
            Price = 200,
            CleaningFee = 20,
            Imagepath = "new/image/path",
            Amenities = new List<Guid> { Guid.NewGuid() }
        };

        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(apartment);
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUser(Guid.NewGuid(), "123", "123"));

        // Act
        var result = await _sut.UpdateApartmentAsync(apartmentId, updateApartmentDto);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.NotAllowedToManageApartment, result.Error);
        _apartmentRepositoryMock.Verify(x => x.Update(It.IsAny<Apartment>()), Times.Never);
    }

    [Fact]
    public async Task DeleteApartmentAsync_ShouldReturnFailure_WhenLoggedUserIsNotOwner()
    {
        // Arrange
        var apartmentOwnerId = Guid.NewGuid().ToString();
        var apartmentId = Guid.NewGuid();
        var apartment = new Apartment("Test Name", "Test Description",
            new Address("Country", "State", "12345", "City", "Street", "123"),
            100, 10, apartmentOwnerId, "image/path");

        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(apartment);
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUser(Guid.NewGuid(), "123", "123"));

        // Act
        var result = await _sut.DeleteApartmentAsync(apartmentId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.NotAllowedToManageApartment, result.Error);
        _apartmentRepositoryMock.Verify(x => x.Delete(It.IsAny<Apartment>()), Times.Never);
    }

    [Fact]
    public async Task GetApartmentsByGuids_ShouldReturnApartmentDtos_WhenApartmentsExist()
    {
        // Arrange
        var apartmentGuids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var apartments = new List<Apartment>
    {
        new Apartment("Test Name 1", "Test Description 1", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, "ownerId1", "image/path1"),
        new Apartment("Test Name 2", "Test Description 2", new Address("Country", "State", "12345", "City", "Street", "123"), 200, 20, "ownerId2", "image/path2")
    };
        var apartmentDtos = apartments.Select(a => new ApartmentDto { Id = a.Id, Name = a.Name, Description = a.Description }).ToList();

        _apartmentRepositoryMock.Setup(x => x.GetApartmentsByGuids(apartmentGuids)).ReturnsAsync(apartments);
       
        // Act
        var result = await _sut.GetApartmentsByGuids(apartmentGuids);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(apartmentDtos.Count, result.Value.Count);
        Assert.Equal(apartmentDtos.First().Name, result.Value.First().Name);
    }

    [Fact]
    public async Task GetApartmentsByUserId_ShouldReturnApartmentDtos_WhenUserHasApartments()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apartments = new List<Apartment>
    {
        new Apartment("Test Name 1", "Test Description 1", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, userId.ToString(), "image/path1"),
        new Apartment("Test Name 2", "Test Description 2", new Address("Country", "State", "12345", "City", "Street", "123"), 200, 20, userId.ToString(), "image/path2")
    };
        var apartmentDtos = apartments.Select(a => new ApartmentDto { Id = a.Id, Name = a.Name, Description = a.Description }).ToList();

        _apartmentRepositoryMock.Setup(x => x.GetApartmentsByUserId(userId)).ReturnsAsync(apartments);
        
        // Act
        var result = await _sut.GetApartmentsByUserId(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(apartmentDtos.Count, result.Value.Count);
        Assert.Equal(apartmentDtos.First().Name, result.Value.First().Name);
    }

    [Fact]
    public async Task PatchApartmentIsActive_ShouldReturnFailure_WhenApartmentDoesNotExist()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(() => null);

        // Act
        var result = await _sut.PatchApartmentIsActive(apartmentId, true);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.ProvidedApartmentNotFound, result.Error);
    }

    [Fact]
    public async Task PatchApartmentIsActive_ShouldReturnFailure_WhenUserIsNotOwner()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        var apartment = new Apartment("Test Name", "Test Description", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, "differentOwnerId", "image/path");

        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(apartment);
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(new CurrentUser(Guid.NewGuid(), "123", "123"));

        // Act
        var result = await _sut.PatchApartmentIsActive(apartmentId, true);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApartmentError.NotAllowedToManageApartment, result.Error);
    }

    [Fact]
    public async Task PatchApartmentIsActive_ShouldReturnSuccess_WhenUserIsOwner()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        var loggedUser = new CurrentUser(Guid.NewGuid(), "123", "123");
        var apartment = new Apartment("Test Name", "Test Description", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, loggedUser.Id.ToString(), "image/path");

        _apartmentRepositoryMock.Setup(x => x.GetByIdAsync(apartmentId)).ReturnsAsync(apartment);
        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(loggedUser); 
        
        // Act
        var result = await _sut.PatchApartmentIsActive(apartmentId, true);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsActive);
        _apartmentRepositoryMock.Verify(x => x.Update(apartment), Times.Once); 
    }

    [Fact]
    public async Task CountUserApartmentsCreated_ShouldReturnCount_WhenUserHasApartments()
    {
        // Arrange
        var loggedUser = new CurrentUser(Guid.NewGuid(), "123", "123");
        var apartmentCount = 3;

        _currentUserServiceMock.Setup(x => x.GetCurrentUser()).Returns(loggedUser);
        _apartmentRepositoryMock.Setup(x => x.CountByUserIdAsync(loggedUser.Id)).ReturnsAsync(apartmentCount);

        // Act
        var result = await _sut.CountUserApartmentsCreated();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(apartmentCount, result.Value);
    }

    [Fact]
    public async Task SearchApartmentsAsync_ShouldReturnFailure_WhenSearchTextIsEmpty()
    {
        // Act
        var result = await _sut.SearchApartmentsAsync("");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ApplicationErrors.ApplicationError.SearchTextCannotBeEmpty, result.Error);
    }

    [Fact]
    public async Task SearchApartmentsAsync_ShouldReturnApartmentDtos_WhenSearchTextMatches()
    {
        // Arrange
        var searchText = "test";
        var apartments = new List<Apartment>
    {
        new Apartment("Test Name 1", "Test Description 1", new Address("Country", "State", "12345", "City", "Street", "123"), 100, 10, "ownerId1", "image/path1"),
        new Apartment("Test Name 2", "Test Description 2", new Address("Country", "State", "12345", "City", "Street", "123"), 200, 20, "ownerId2", "image/path2")
    };
        var apartmentDtos = apartments.Select(a => new ApartmentDto { Id = a.Id, Name = a.Name, Description = a.Description }).ToList();

        _apartmentRepositoryMock.Setup(x => x.SearchByTextAsync(searchText)).ReturnsAsync(apartments);
        
        // Act
        var result = await _sut.SearchApartmentsAsync(searchText);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(apartmentDtos.Count, result.Value.Count);
        Assert.Equal(apartmentDtos.First().Name, result.Value.First().Name);
    }

}
