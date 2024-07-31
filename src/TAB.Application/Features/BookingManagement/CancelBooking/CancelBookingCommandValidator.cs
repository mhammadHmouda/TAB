using FluentValidation;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0).WithMessage("The booking id is required.");
    }
}
