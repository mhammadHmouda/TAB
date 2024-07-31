using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public record CancelBookingCommand(int BookingId) : ICommand<Result>;
