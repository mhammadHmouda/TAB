using FluentValidation;

namespace TAB.Application.Features.ReviewManagement.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Review id is required.");
        RuleFor(x => x.Title).NotEmpty().WithMessage("Review title is required.");
        RuleFor(x => x.Content).NotEmpty().WithMessage("Review content is required.");
        RuleFor(x => x.Rating)
            .NotEmpty()
            .WithMessage("Review rating is required.")
            .GreaterThan(0)
            .WithMessage("Review rating must be greater than 0.")
            .LessThan(6)
            .WithMessage("Review rating must be less than 6.");
    }
}
