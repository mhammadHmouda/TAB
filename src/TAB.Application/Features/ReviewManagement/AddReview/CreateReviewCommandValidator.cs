using FluentValidation;

namespace TAB.Application.Features.ReviewManagement.AddReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Review title is required.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Review content is required.");
        RuleFor(x => x.Rating)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .WithMessage("Review rating must be greater than or equal to 1.")
            .LessThanOrEqualTo(5)
            .WithMessage("Review rating must be less than or equal to 5.");
        RuleFor(x => x.HotelId).NotEmpty().GreaterThan(0).WithMessage("Hotel id is required.");
        RuleFor(x => x.UserId).NotEmpty().GreaterThan(0).WithMessage("User id is required.");
    }
}
