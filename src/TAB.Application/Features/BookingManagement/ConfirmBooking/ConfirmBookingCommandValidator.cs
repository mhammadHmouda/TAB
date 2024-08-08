using FluentValidation;

namespace TAB.Application.Features.BookingManagement.ConfirmBooking;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .GreaterThan(0)
            .WithMessage("The booking id must be greater than 0.");
    }
}
