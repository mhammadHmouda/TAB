using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Amenities.AddAmenity;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Services;

namespace Application.UnitTests.Features.HotelManagement.Amenities;

public class CreateAmenityCommandTests
{
    private readonly IAmenityRepository _amenityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IAmenityService _amenityServiceMock;
    private readonly IUserContext _userContext;

    private static readonly CreateAmenityCommand Command =
        new("Amenity Name", "Amenity Description", AmenityType.Hotel, 1);

    private readonly CreateAmenityCommandHandler _sut;

    public CreateAmenityCommandTests()
    {
        _amenityRepositoryMock = Substitute.For<IAmenityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _amenityServiceMock = Substitute.For<IAmenityService>();
        _userContext = Substitute.For<IUserContext>();

        _sut = new CreateAmenityCommandHandler(
            _amenityRepositoryMock,
            _unitOfWorkMock,
            _userContext,
            _amenityServiceMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserIsAdmin_ShouldReturnSuccessResult()
    {
        // Arrange
        _userContext.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckFails_ShouldReturnError()
    {
        // Arrange
        _userContext.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(DomainErrors.Amenity.NothingToUpdate);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Amenity.NothingToUpdate);
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckSucceeds_ShouldReturnSuccessResult()
    {
        // Arrange
        _userContext.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckSucceeds_ShouldCreateAmenity()
    {
        // Arrange
        _userContext.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _amenityRepositoryMock.Received(1).InsertAsync(Arg.Any<Amenity>());
    }

    [Fact]
    public async Task Handle_WhenUserIsAdminAndAmenityTypeAndUserOwnershipCheckSucceeds_ShouldSaveChanges()
    {
        // Arrange
        _userContext.Id.Returns(1);

        _amenityServiceMock
            .CheckAmenityTypeAndUserOwnerShipAsync(1, 1, AmenityType.Hotel, CancellationToken.None)
            .Returns(Result.Success());

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
