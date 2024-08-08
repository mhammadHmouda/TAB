using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Amenities.DeleteAmenity;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace Application.UnitTests.Features.HotelManagement.Amenities;

public class DeleteAmenityCommandTests
{
    private readonly IAmenityRepository _amenityRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly DeleteAmenityCommand Command = new(1);

    private readonly DeleteAmenityCommandHandler _sut;

    public DeleteAmenityCommandTests()
    {
        _amenityRepositoryMock = Substitute.For<IAmenityRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new DeleteAmenityCommandHandler(_amenityRepositoryMock, _unitOfWorkMock);

        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(
                Maybe<Amenity>.From(Amenity.Create("name", "description", AmenityType.Hotel, 1))
            );
    }

    [Fact]
    public async Task Handle_WhenAmenityDoesNotExist_ReturnsNotFound()
    {
        _amenityRepositoryMock
            .GetByIdAsync(Command.Id, CancellationToken.None)
            .Returns(Maybe<Amenity>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.Amenity.NotFound);
    }

    [Fact]
    public async Task Handle_WhenAmenityExists_ReturnsSuccess()
    {
        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _amenityRepositoryMock.Received(1).Delete(Arg.Any<Amenity>());
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
