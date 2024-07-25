using TAB.Application.Core.Contracts;
using TAB.Application.Core.Extensions;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.ReviewManagement.UpdateReview;

public record UpdateReviewCommand(int Id, string Title, string Content, int Rating)
    : ICommand<Result<ReviewResponse>>;
