using FluentValidation;

namespace TAB.Application.Features.BookingManagement.SuccessPayment;

public class SuccessPaymentCommandValidator : AbstractValidator<SuccessPaymentCommand>
{
    public SuccessPaymentCommandValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty().WithMessage("SessionId is required.");
    }
}
