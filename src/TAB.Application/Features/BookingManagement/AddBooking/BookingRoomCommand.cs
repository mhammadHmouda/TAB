using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.BookingManagement.AddBooking;

public record BookingRoomCommand(DateTime CheckInDate, DateTime CheckOutDate, int RoomId)
    : ICommand<Result<BookingResponse>>;
