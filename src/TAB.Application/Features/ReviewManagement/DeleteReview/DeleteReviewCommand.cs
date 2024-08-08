using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.ReviewManagement.DeleteReview;

public record DeleteReviewCommand(int Id) : ICommand<Result>;
