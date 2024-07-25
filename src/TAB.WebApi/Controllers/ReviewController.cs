using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.ReviewManagement.AddReview;
using TAB.Application.Features.ReviewManagement.DeleteReview;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing reviews.
/// </summary>
[TokenValidation]
public class ReviewController : ApiController
{
    [HttpPost(ApiRoutes.Review.Create)]
    public async Task<IActionResult> CreateReview(
        [FromQuery] [Required] int hotelId,
        [FromQuery] [Required] int userId,
        [FromBody] CreateReviewRequest request
    ) =>
        await Result
            .Create((hotelId, userId, request))
            .Ensure(x => x.hotelId == request.HotelId, DomainErrors.General.UnProcessableRequest)
            .Ensure(x => x.userId == request.UserId, DomainErrors.General.UnProcessableRequest)
            .Map(x => new CreateReviewCommand(
                x.request.Title,
                x.request.Content,
                x.request.Rating,
                x.hotelId,
                x.userId
            ))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    [HttpDelete(ApiRoutes.Review.Delete)]
    public async Task<IActionResult> DeleteReview(int id) =>
        await Result
            .Create(id)
            .Map(i => new DeleteReviewCommand(i))
            .Bind(x => Mediator.Send(x))
            .Match(() => Ok("Review deleted successfully!"), BadRequest);
}
