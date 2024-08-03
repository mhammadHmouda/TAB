using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Amenities.UpdateAmenity;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace Application.UnitTests.Features.HotelManagement.Amenities;

public class UpdateAmenityCommandTests
{
    private readonly IAmenityRepository _amenityRepositoryMock;

    private static readonly UpdateAmenityCommand Command =
        new(1, "Amenity Name", "Amenity Description");
    private readonly UpdateAmenityCommandHandler _sut;

    public UpdateAmenityCommandTests()
    {
        _amenityRepositoryMock = Substitute.For<IAmenityRepository>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        Substitute.For<IUserContext>();

        _sut = new UpdateAmenityCommandHandler(
            _amenityRepositoryMock,
            unitOfWorkMock,
            Substitute.For<IMapper>()
        );
    }

    [Fact]
    public async Task Handle_WhenAmenityDoesNotExist_ReturnsNotFoundError()
    {
        // Arrange
        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(Maybe<Amenity>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Amenity.NotFound);
    }

    [Theory]
    [InlineData(AmenityType.Hotel)]
    [InlineData(AmenityType.Room)]
    public async Task Handle_WhenAmenityExists_ReturnsUpdatedAmenityResponse(AmenityType type)
    {
        // Arrange
        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(Amenity.Create("Old Name", "Old Description", type, 1));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<AmenityResponse>();
    }

    [Theory]
    [InlineData(AmenityType.Hotel)]
    [InlineData(AmenityType.Room)]
    public async Task Handle_WhenAmenityExists_ReturnsUpdatedAmenityResponseWithSameType(
        AmenityType type
    )
    {
        // Arrange
        var amenity = Amenity.Create("Old Name", "Old Description", type, 1);

        _amenityRepositoryMock.GetByIdAsync(Command.Id, CancellationToken.None).Returns(amenity);

        amenity.Update(Command.Name, Command.Description);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Amenity.NothingToUpdate);
    }
}
