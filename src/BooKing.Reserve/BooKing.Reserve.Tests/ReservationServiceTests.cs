using AutoMapper;
using BooKing.Generics.Domain;
using BooKing.Generics.EventSourcing;
using BooKing.Generics.Outbox.Events;
using BooKing.Generics.Outbox.Service;
using BooKing.Generics.Shared.CurrentUserService;
using BooKing.Reserve.Application;
using BooKing.Reserve.Application.Dtos;
using BooKing.Reserve.Application.Erros;
using BooKing.Reserve.Application.Exceptions;
using BooKing.Reserve.Application.Interfaces;
using BooKing.Reserve.Application.Services;
using BooKing.Reserve.Domain;
using BooKing.Reserve.Domain.Entities;
using BooKing.Reserve.Domain.Enums;
using BooKing.Reserve.Domain.Errors;
using BooKing.Reserve.Domain.Interfaces;
using BooKing.Reserve.Domain.ValueObjects;
using Moq;

namespace BooKing.Reserve.Tests;

public class ReservationServiceTests
{
    private readonly Mock<IReservationRepository> _reservationRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IApartmentService> _apartmentServiceMock;
    private readonly Mock<IOutboxEventService> _outboxEventServiceMock;
    private readonly IMapper _mapper;
    private readonly Mock<IEventSourcingRepository> _eventSourcingRepositoryMock;
    private readonly Mock<PricingService> _pricingServiceMock;

    private readonly ReservationService _sut;

    public ReservationServiceTests()
    {
        _reservationRepositoryMock = new Mock<IReservationRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _apartmentServiceMock = new Mock<IApartmentService>();
        _outboxEventServiceMock = new Mock<IOutboxEventService>();
        _mapper = AutoMapperConfiguration.Create().CreateMapper();
        _eventSourcingRepositoryMock = new Mock<IEventSourcingRepository>();
        _pricingServiceMock = new Mock<PricingService>();

        _sut = new ReservationService(
            _reservationRepositoryMock.Object,
            _currentUserServiceMock.Object,
            _apartmentServiceMock.Object,
            _pricingServiceMock.Object,
            _outboxEventServiceMock.Object,
            _mapper,
            _eventSourcingRepositoryMock.Object
        );
    }


    [Fact]
    public async Task Reserve_ShouldFail_When_ApartmentService_Returns_Failure()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();

        var duration = DateRange.Create(DateTime.Now, DateTime.Now.AddDays(2)).Value;

        _apartmentServiceMock.Setup(a => a.GetApartment(apartmentId))
            .ReturnsAsync(Result.Failure<GetApartmentDto>(Error.NullValue));

        // Act
        var result = await _sut.Reserve(new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = duration.Start,
            EndDate = duration.End
        });

        // Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Reserve_ShouldFail_WhenApartmentIsInactiveOrDeleted()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();

        var apartment = new GetApartmentDto
        {
            Id = apartmentId,
            Name = "Test Apartment",
            OwnerId = Guid.NewGuid().ToString(),
            Price = 200,
            CleaningFee = 50,
            IsActive = false,
            IsDeleted = false
        };

        _apartmentServiceMock.Setup(a => a.GetApartment(apartmentId))
            .ReturnsAsync(Result.Success(apartment));

        // Act
        var result = await _sut.Reserve(new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2)
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.ApartmentInactive, result.Error);
    }

    [Fact]
    public async Task Reserve_ShouldFail_WhenUserNotAllowedToReserveYourOwnApartment()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var apartment = new GetApartmentDto
        {
            Id = apartmentId,
            Name = "Test Apartment",
            OwnerId = userId.ToString(),
            Price = 200,
            CleaningFee = 50,
            IsActive = true,
            IsDeleted = false
        };

        var currentUser = new CurrentUser(userId, "user@example.com", "token");

        _currentUserServiceMock.Setup(c => c.GetCurrentUser())
            .Returns(currentUser);

        _apartmentServiceMock.Setup(a => a.GetApartment(apartmentId))
            .ReturnsAsync(Result.Success(apartment));

        // Act
        var result = await _sut.Reserve(new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2)
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotAllowedToReserveYourOwnApartment, result.Error);
    }

    [Fact]
    public async Task Reserve_ShouldFail_WhenDateRangeIsInvalid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var apartment = new GetApartmentDto
        {
            Id = apartmentId,
            Name = "Test Apartment",
            OwnerId = Guid.NewGuid().ToString(),
            Price = 200,
            CleaningFee = 50,
            IsActive = true,
            IsDeleted = false
        };

        var currentUser = new CurrentUser(userId, "user@example.com", "token");

        _currentUserServiceMock.Setup(c => c.GetCurrentUser())
            .Returns(currentUser);

        _apartmentServiceMock.Setup(a => a.GetApartment(apartmentId))
            .ReturnsAsync(Result.Success(apartment));

        // Act
        var result = await _sut.Reserve(new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = DateTime.Now.AddDays(5),
            EndDate = DateTime.Now
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(DomainErrors.ReserveError.EndDatePrecedesStartDate, result.Error);
    }

    [Fact]
    public async Task Reserve_ShouldFail_WhenReservationConflicts()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var apartment = new GetApartmentDto
        {
            Id = apartmentId,
            Name = "Test Apartment",
            OwnerId = Guid.NewGuid().ToString(),
            Price = 200,
            CleaningFee = 50,
            IsActive = true,
            IsDeleted = false
        };

        var currentUser = new CurrentUser(userId, "user@example.com", "token");

        _currentUserServiceMock.Setup(c => c.GetCurrentUser())
            .Returns(currentUser);

        var duration = DateRange.Create(DateTime.Now, DateTime.Now.AddDays(2)).Value;

        _apartmentServiceMock.Setup(a => a.GetApartment(apartmentId))
            .ReturnsAsync(Result.Success(apartment));

        _reservationRepositoryMock.Setup(r => r.IsOverlappingAsync(apartmentId, duration))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.Reserve(new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = duration.Start,
            EndDate = duration.End
        });

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.Overlap, result.Error);
    }

    [Fact]
    public async Task Reserve_ShouldReturnOverlapError_WhenConcurrencyExceptionIsThrown()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(5)).Value;

        var apartment = new GetApartmentDto
        {
            Id = apartmentId,
            IsActive = true,
            OwnerId = Guid.NewGuid().ToString(),
            Price = 100,
            CleaningFee = 50
        };

        var newReservationDto = new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = duration.Start,
            EndDate = duration.End
        };

        var currentUser = new CurrentUser(userId, "user@example.com", "token");

        _currentUserServiceMock.Setup(c => c.GetCurrentUser())
            .Returns(currentUser);

        _apartmentServiceMock
            .Setup(service => service.GetApartment(apartmentId))
            .ReturnsAsync(Result.Success(apartment));

        _reservationRepositoryMock
            .Setup(repo => repo.IsOverlappingAsync(apartmentId, duration))
            .ReturnsAsync(false);

        _reservationRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Reservation>()))
            .ThrowsAsync(new ConcurrencyException("", new Exception()));

        // Act
        var result = await _sut.Reserve(newReservationDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.Overlap, result.Error);
    }

    [Fact]
    public async Task Reserve_ShouldCreateReservation_WhenInputIsValid()
    {
        // Arrange
        var apartmentId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var apartment = new GetApartmentDto
        {
            Id = apartmentId,
            Name = "apartment",
            OwnerId = Guid.NewGuid().ToString(),
            Price = 200,
            CleaningFee = 50,
            IsActive = true,
            IsDeleted = false
        };

        var currentUser = new CurrentUser(userId, "user@example.com", "token");

        var duration = DateRange.Create(DateTime.Now, DateTime.Now.AddDays(5)).Value;

        var pricingDetails = new PricingDetails(
            PriceForPeriod: 1000,
            CleaningFee: 50,
            TotalPrice: 1050);

        _apartmentServiceMock.Setup(a => a.GetApartment(apartmentId))
            .ReturnsAsync(Result.Success(apartment));

        _currentUserServiceMock.Setup(c => c.GetCurrentUser())
            .Returns(currentUser);

        _reservationRepositoryMock.Setup(r => r.IsOverlappingAsync(apartmentId, duration))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.Reserve(new NewReservationDto
        {
            ApartmentId = apartmentId,
            StartDate = duration.Start,
            EndDate = duration.End
        });

        // Assert
        Assert.True(result.IsSuccess);

        _reservationRepositoryMock.Verify(r => r.AddAsync(It.Is<Reservation>(r =>
            r.ApartmentId == apartmentId &&
            r.UserId == userId &&
            r.PriceForPeriod == pricingDetails.PriceForPeriod &&
            r.CleaningFee == pricingDetails.CleaningFee &&
            r.TotalPrice == pricingDetails.TotalPrice &&
            r.Duration == duration
        )), Times.Once);

        _outboxEventServiceMock.Verify(o => o.AddEvent(It.IsAny<ReservationCreatedEvent>()), Times.Once);
    }

    [Fact]
    public async Task ConfirmReserve_ShouldReturnFailure_WhenReservationNotFound()
    {
        // Arrange
        _reservationRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Reservation)null);

        // Act
        var result = await _sut.ConfirmReserve(Guid.NewGuid());

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotFound, result.Error);
    }

    [Fact]
    public async Task ConfirmReserve_ShouldReturnFailure_WhenUserNotAllowedToConfirm()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var anotherUserId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
        var price = 100m;
        var cleaningFee = 50m;
        var currentUser = new CurrentUser(Guid.NewGuid(), "user@example.com", "token");
        var pricingDetails = new PricingDetails(
            PriceForPeriod: 200m,
            CleaningFee: 50m,
            TotalPrice: 250m
        );

        var reservation = Reservation.Reserve(
            apartmentId,
            anotherUserId,
            price,
            cleaningFee,
            duration.Value,
            _pricingServiceMock.Object
        );

        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);


        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        // Act
        var result = await _sut.ConfirmReserve(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotAllowedToConfirmReservation, result.Error);
    }

    [Fact]
    public async Task ConfirmReserve_ShouldReturnFailure_WhenInvalidStatus()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
        var price = 100m;
        var cleaningFee = 50m;

        var reservation = Reservation.Reserve(
            apartmentId,
            userId,
            price,
            cleaningFee,
            duration.Value,
            _pricingServiceMock.Object
        );
        reservation.Cancel();

        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(new CurrentUser(userId, "user@example.com", "token"));

        // Act
        var result = await _sut.ConfirmReserve(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.InvalidStatus, result.Error);
    }

    [Fact]
    public async Task ConfirmReserve_ShouldSucceed_WhenReservationCanBeConfirmed()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
        var price = 100m;
        var cleaningFee = 50m;

        var reservation = Reservation.Reserve(
            apartmentId,
            userId,
            price,
            cleaningFee,
            duration.Value,
            _pricingServiceMock.Object
        );

        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(new CurrentUser(userId, "user@example.com", "token"));

        // Act
        var result = await _sut.ConfirmReserve(reservationId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ReservationStatus.Confirmed, reservation.Status);
        _reservationRepositoryMock.Verify(repo => repo.Update(reservation), Times.Once);
    }

    [Fact]
    public async Task CancelReserve_ShouldReturnFailure_WhenReservationAlreadyCancelled()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
        var price = 100m;
        var cleaningFee = 50m;
        var currentUser = new CurrentUser(userId, "user@example.com", "token");
        var reservation = Reservation.Reserve(
            apartmentId,
            userId,
            price,
            cleaningFee,
            duration.Value,
            _pricingServiceMock.Object
        );
        reservation.Cancel();

        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        // Act
        var result = await _sut.CancelReserve(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.InvalidStatus, result.Error);
    }

    [Fact]
    public async Task CancelReserve_ShouldReturnFailure_WheReserveNotFound()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Reservation)null);

        // Act
        var result = await _sut.CancelReserve(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotFound, result.Error);
    }

    [Fact]
    public async Task CancelReserve_ShouldReturnFailure_WheUserNotAllowedToConfirmReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
        var price = 100m;
        var cleaningFee = 50m;
        var currentUser = new CurrentUser(userId, "user@example.com", "token");
        var reservation = Reservation.Reserve(
            apartmentId,
            Guid.NewGuid(),
            price,
            cleaningFee,
            duration.Value,
            _pricingServiceMock.Object
        );

        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        // Act
        var result = await _sut.CancelReserve(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotAllowedToCancelReservation, result.Error);
    }

    [Fact]
    public async Task CancelReserve_ShouldSucceed_WhenReservationCanBeCancelled()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var duration = DateRange.Create(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(3));
        var price = 100m;
        var cleaningFee = 50m;
        var currentUser = new CurrentUser(userId, "user@example.com", "token");
        var reservation = Reservation.Reserve(
            apartmentId,
            userId,
            price,
            cleaningFee,
            duration.Value,
            _pricingServiceMock.Object
        );

        _reservationRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        // Act
        var result = await _sut.CancelReserve(reservationId);

        // Assert
        Assert.True(result.IsSuccess);
        _reservationRepositoryMock.Verify(repo => repo.Update(reservation), Times.Once);
    }

    [Fact]
    public async Task GetAllReservationsByUserId_ShouldReturnFailure_WhenUserNotAllowedToRetrieveInformation()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser(Guid.NewGuid(), "user@example.com", "token");

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        // Act
        var result = await _sut.GetAllReservationsByUserId(userId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation, result.Error);
    }

    [Fact]
    public async Task GetAllReservationsByUserId_ShouldReturnFailure_WhenErrorObtainingApartmentData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser(userId, "user@example.com", "token");
        var reservations = new List<Reservation>
    {
        Reservation.Reserve(Guid.NewGuid(), userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object)
    };

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        _reservationRepositoryMock
            .Setup(repo => repo.GetAllReservationsByUserId(It.IsAny<Guid>()))
            .ReturnsAsync(reservations);

        _apartmentServiceMock
            .Setup(service => service.GetApartmentByGuidList(It.IsAny<List<Guid>>()))
            .ReturnsAsync(Result.Failure<List<GetApartmentDto>>(ApplicationErrors.ReserveError.ErrorObtainingApartmentData));

        // Act
        var result = await _sut.GetAllReservationsByUserId(userId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.ErrorObtainingApartmentData, result.Error);
    }
    
    [Fact]
    public async Task GetAllReservationsByUserId_ShouldReturnSuccess_WithMappedReservations()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var currentUser = new CurrentUser(userId, "user@example.com", "token");
        var apartmentId = Guid.NewGuid();
        var reservations = new List<Reservation>
        {
            Reservation.Reserve(apartmentId, userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object)
        };
        var apartments = new List<GetApartmentDto>()
        { 
            new GetApartmentDto() { Id = apartmentId }
        };

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        _reservationRepositoryMock
            .Setup(repo => repo.GetAllReservationsByUserId(It.IsAny<Guid>()))
            .ReturnsAsync(reservations);

        _apartmentServiceMock
            .Setup(service => service.GetApartmentByGuidList(It.IsAny<List<Guid>>()))
            .ReturnsAsync(Result.Success(apartments));

        // Act
        var result = await _sut.GetAllReservationsByUserId(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetReservation_ShouldReturnFailure_WhenReservationNotFound()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync((Reservation)null);

        // Act
        var result = await _sut.GetReservation(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotFound, result.Error);
    }

    [Fact]
    public async Task GetReservation_ShouldReturnFailure_WhenUserNotAllowedToRetrieveInformation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reservation = Reservation.Reserve(Guid.NewGuid(), userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object);
        var currentUser = new CurrentUser(Guid.NewGuid(), "user@example.com", "token");

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        // Act
        var result = await _sut.GetReservation(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation, result.Error);
    }

    [Fact]
    public async Task GetReservation_ShouldReturnFailure_WhenErrorObtainingApartmentData()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var reservation = Reservation.Reserve(apartmentId, userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object);
        var currentUser = new CurrentUser(userId, "user@example.com", "token");

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        _apartmentServiceMock
            .Setup(service => service.GetApartment(It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure<GetApartmentDto>(ApplicationErrors.ReserveError.ErrorObtainingApartmentData));

        // Act
        var result = await _sut.GetReservation(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.ErrorObtainingApartmentData, result.Error);
    }

    [Fact]
    public async Task GetReservation_ShouldReturnSuccess_WithMappedReservationDto()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var apartmentId = Guid.NewGuid();
        var reservation = Reservation.Reserve(apartmentId, userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object);
        var currentUser = new CurrentUser(userId, "user@example.com", "token");
        var apartment = new GetApartmentDto { Id = apartmentId };

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(currentUser);

        _apartmentServiceMock
            .Setup(service => service.GetApartment(It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(apartment));

        // Act
        var result = await _sut.GetReservation(reservationId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetReservationEvents_ShouldReturnFailure_WhenReservationNotFound()
    {
        // Arrange
        var reservationId = Guid.NewGuid();

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync((Reservation)null);

        // Act
        var result = await _sut.GetReservationEvents(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotFound, result.Error);
    }

    [Fact]
    public async Task GetReservationEvents_ShouldReturnFailure_WhenEventsNotFound()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reservation = Reservation.Reserve(Guid.NewGuid(), userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object);

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(new CurrentUser(userId, "user@example.com", "token"));

        _eventSourcingRepositoryMock
            .Setup(repo => repo.GetEvents(It.IsAny<Guid>()))
            .ReturnsAsync((List<StoredEvent>)null);

        // Act
        var result = await _sut.GetReservationEvents(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.EventsNotFound, result.Error);
    }

    [Fact]
    public async Task GetReservationEvents_ShouldReturnFailure_WhenUserIsNowAllowed()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reservation = Reservation.Reserve(Guid.NewGuid(), userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object);

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(new CurrentUser(Guid.NewGuid(), "user@example.com", "token"));

        // Act
        var result = await _sut.GetReservationEvents(reservationId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ApplicationErrors.ReserveError.NotAllowedToRetrieveThisInformation, result.Error);
    }

    [Theory]
    [InlineData("ReservationCreatedEvent", "", null)]
    [InlineData("ReservationConfirmedByUserEvent", "Reservation confirmed by user", null)]
    [InlineData("ReservationCancelledByUserEvent", "Reservation Cancelled by user", null)]
    [InlineData("ReservationPaymentInitiatedEvent", "", null)]
    [InlineData("ReservationPaymentProcessedEvent", "Payment status: Approved", "{\"IsApproved\":true}")]
    [InlineData("ReservationPaymentProcessedEvent", "Payment status: Rejected", "{\"IsApproved\":false}")]
    [InlineData("ReservationPaymentProcessedEvent", "", null)]
    [InlineData("ReservationReservedEvent", "Reservation process finalized", null)]
    [InlineData("ReservationCompletedEvent", "Reservation Completed!", null)]
    [InlineData("UnknownEventType", "Unknown Event", null)]
    public async Task GetReservationEvents_ShouldReturnSuccess_WithMappedEvents(string eventType, string expected, string data)
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var reservation = Reservation.Reserve(Guid.NewGuid(), userId, 100, 50, DateRange.Create(DateTime.UtcNow, DateTime.UtcNow.AddDays(2)).Value, _pricingServiceMock.Object);
        var events = new List<StoredEvent>
        {
            new StoredEvent(Guid.NewGuid(), eventType, data, DateTime.UtcNow)
        };

        _reservationRepositoryMock
            .Setup(repo => repo.GetReservation(It.IsAny<Guid>()))
            .ReturnsAsync(reservation);

        _currentUserServiceMock
            .Setup(service => service.GetCurrentUser())
            .Returns(new CurrentUser(userId, "user@example.com", "token"));

        _eventSourcingRepositoryMock
            .Setup(repo => repo.GetEvents(It.IsAny<Guid>()))
            .ReturnsAsync(events);

        // Act
        var result = await _sut.GetReservationEvents(reservationId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result);
        Assert.Equal(expected, result.Value[0].AdditionalInformation);

    }

    [Fact]
    public async Task CountUserReservations_ShouldReturnCorrectCount()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedCount = 5;

        _currentUserServiceMock.Setup(service => service.GetCurrentUser()).Returns(new CurrentUser(userId, "user@example.com", "token"));

        _reservationRepositoryMock.Setup(repo => repo.CountByUserIdAsync(userId)).ReturnsAsync(expectedCount);

        // Act
        var result = await _sut.CountUserReservations();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedCount, result.Value);
    }
}