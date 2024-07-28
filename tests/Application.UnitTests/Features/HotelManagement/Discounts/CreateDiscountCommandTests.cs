using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Discounts.AddDiscount;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.UserManagement.Enums;

namespace Application.UnitTests.Features.HotelManagement.Discounts;

public class CreateDiscountHandlerTests
{
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IDateTimeProvider _dateTimeProviderMock;

    private static readonly CreateDiscountCommand Command =
        new(1, "Name", "Description", 10, DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(6));

    private readonly CreateDiscountCommandHandler _sut;

    public CreateDiscountHandlerTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

        _sut = new CreateDiscountCommandHandler(
            _roomRepositoryMock,
            _unitOfWorkMock,
            _dateTimeProviderMock
        );
    }

    [Fact]
    public async Task Handle_WhenRoomExists_ReturnsDiscountResponse()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(1, Money.Create(1, "USD"), "Room Description", RoomType.Single, 1, 1)
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DiscountResponse>();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_AddsDiscountToRoom()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(1, Money.Create(1, "USD"), "Room Description", RoomType.Single, 1, 1)
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DiscountResponse>();

        var discount = result.Value;

        discount.DiscountPercentage.Should().Be(Command.DiscountPercentage);
        discount.StartDate.Should().Be(Command.StartDate);
        discount.EndDate.Should().Be(Command.EndDate);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_CallsUnitOfWorkSaveChangesAsync()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(1, Money.Create(1, "USD"), "Room Description", RoomType.Single, 1, 1)
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<DiscountResponse>();

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
