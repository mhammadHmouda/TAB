using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.ReviewManagement.Repositories;

namespace TAB.Application.Features.ReviewManagement.UpdateReview;

public class UpdateReviewCommandHandler
    : ICommandHandler<UpdateReviewCommand, Result<ReviewResponse>>
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(
        IReviewRepository reviewRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork
    )
    {
        _reviewRepository = reviewRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReviewResponse>> Handle(
        UpdateReviewCommand command,
        CancellationToken cancellationToken
    )
    {
        var maybeReview = await _reviewRepository.GetByIdAsync(command.Id, cancellationToken);

        if (maybeReview.HasNoValue)
        {
            return DomainErrors.Review.NotFound;
        }

        var review = maybeReview.Value;
        var userId = _userContext.Id;

        if (review.UserId != userId)
        {
            return DomainErrors.General.Unauthorized;
        }

        var result = review.Update(command.Title, command.Content, command.Rating);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ReviewResponse(
            review.Id,
            review.Title,
            review.Content,
            review.Rating,
            review.HotelId,
            review.UserId
        );
    }
}
