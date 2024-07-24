using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Discounts.AddDiscount;

public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
{
    public CreateDiscountCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The discount name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("The discount description is required.");
        RuleFor(x => x.DiscountPercentage)
            .NotEmpty()
            .WithMessage("The discount percentage is required.")
            .Must(x => x is >= 0 and <= 100)
            .WithMessage("The discount percentage must be between 0 and 100.");
        RuleFor(x => x.StartDate)
            .Must((x, start) => start < x.EndDate)
            .WithMessage("The start date must be before the end date.");
    }
}
