using FluentValidation;

namespace TAB.Application.Features.BookingManagement.CheckoutRoom;

public class CheckoutBookingCommandValidator : AbstractValidator<CheckoutBookingCommand>
{
    public CheckoutBookingCommandValidator()
    {
        RuleFor(x => x.BookingId).GreaterThan(0).WithMessage("The booking id is required.");
    }
}
