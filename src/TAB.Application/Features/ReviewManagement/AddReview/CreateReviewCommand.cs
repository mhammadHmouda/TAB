using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.ReviewManagement.AddReview;

public record CreateReviewCommand(string Title, string Content, int Rating, int HotelId, int UserId)
    : ICommand<Result<ReviewResponse>>;
