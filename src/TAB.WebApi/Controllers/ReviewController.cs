﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.ReviewManagement.AddReview;
using TAB.Application.Features.ReviewManagement.DeleteReview;
using TAB.Application.Features.ReviewManagement.GetHotelReviews;
using TAB.Application.Features.ReviewManagement.UpdateReview;
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
    /// <summary>
    /// Creates a new review.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="request">The review request.</param>
    /// <response code="200">The review was created successfully.</response>
    /// <response code="400">The review was not created successfully.</response>
    /// <returns>The result of the operation.</returns>
    [HttpPost(ApiRoutes.Review.Create)]
    public async Task<IActionResult> CreateReview(
        int hotelId,
        int userId,
        CreateReviewRequest request
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

    /// <summary>
    /// Deletes a review.
    /// </summary>
    /// <param name="id">The ID of the review.</param>
    /// <response code="200">The review was deleted successfully.</response>
    /// <response code="400">The review was not deleted successfully.</response>
    /// <returns>The result of the operation.</returns>
    [HttpDelete(ApiRoutes.Review.Delete)]
    public async Task<IActionResult> DeleteReview(int id) =>
        await Result
            .Create(id)
            .Map(i => new DeleteReviewCommand(i))
            .Bind(x => Mediator.Send(x))
            .Match(() => Ok("Review deleted successfully!"), BadRequest);

    /// <summary>
    /// Updates a review.
    /// </summary>
    /// <param name="id">The ID of the review.</param>
    /// <param name="request">The review request.</param>
    /// <response code="200">The review was updated successfully.</response>
    /// <response code="400">The review was not updated successfully.</response>
    /// <returns>The result of the operation.</returns>
    [HttpPut(ApiRoutes.Review.Update)]
    public async Task<IActionResult> UpdateReview(int id, UpdateReviewRequest request)
    {
        return await Result
            .Create((id, request))
            .Map(x => new UpdateReviewCommand(
                x.id,
                x.request.Title,
                x.request.Content,
                x.request.Rating
            ))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
    }

    /// <summary>
    /// Gets all reviews for a hotel.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="sorting">The sorting.</param>
    /// <response code="200">The reviews were retrieved successfully.</response>
    /// <response code="400">The reviews were not retrieved successfully.</response>
    /// <returns>The result of the operation.</returns>
    [HttpGet(ApiRoutes.Review.GetHotelReviews)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetHotelReviews(
        int hotelId,
        string? filters,
        string? sorting,
        int page = 1,
        int pageSize = 10
    ) =>
        await Result
            .Create((hotelId, page, pageSize, filters, sorting))
            .Map(x => new GetHotelReviewsQuery(x.hotelId, x.page, x.pageSize, x.filters, x.sorting))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
