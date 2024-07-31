using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.BookingManagement.ConfirmBooking;

public record ConfirmBookingCommand(int BookingId) : ICommand<Result>;
