using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.ReviewManagement.Events;
using TAB.Domain.Features.ReviewManagement.Repositories;

namespace TAB.Application.Features.ReviewManagement.DeleteReview;

public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand, Result>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork
    )
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteReviewCommand command,
        CancellationToken cancellationToken
    )
    {
        var userId = _userContext.Id;

        var maybeReview = await _reviewRepository.GetByIdAsync(command.Id, cancellationToken);

        if (maybeReview.HasNoValue)
        {
            return DomainErrors.Review.NotFound;
        }

        var review = maybeReview.Value;

        if (review.UserId != userId)
        {
            return DomainErrors.General.Unauthorized;
        }

        _reviewRepository.Remove(review);

        review.AddDomainEvent(new ReviewDeletedEvent(review.HotelId, review.Id));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
