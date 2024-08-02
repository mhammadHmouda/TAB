using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.BookingManagement.AddBooking;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Application.Features.BookingManagement.CheckoutRoom;
using TAB.Application.Features.BookingManagement.ConfirmBooking;
using TAB.Application.Features.BookingManagement.SearchBooking;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

[TokenValidation]
public class BookingController : ApiController
{
    /// <summary>
    /// Book a room.
    /// </summary>
    /// <param name="request">The booking request.</param>
    /// <returns>The booking response.</returns>
    /// <response code="200">The booking was successful.</response>
    /// <response code="400">The booking request is invalid.</response>
    [HttpPost(ApiRoutes.Booking.Create)]
    public async Task<IActionResult> CreateBooking(BookingRoomRequest request)
    {
        return await Result
            .Create(request)
            .Map(r => new BookingRoomCommand(r.CheckInDate, r.CheckOutDate, r.RoomId))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);
    }

    /// <summary>
    /// Confirm a booking.
    /// </summary>
    /// <param name="id">The booking id.</param>
    /// <returns>The result of the confirmation.</returns>
    /// <response code="200">The booking was confirmed.</response>
    /// <response code="400">The booking id is invalid.</response>
    [Authorize(Roles = "Admin")]
    [HttpPut(ApiRoutes.Booking.Confirm)]
    public async Task<IActionResult> ConfirmBooking(int id)
    {
        return await Result
            .Create(id)
            .Map(i => new ConfirmBookingCommand(i))
            .Bind(command => Mediator.Send(command))
            .Match(() => Ok("Booking confirmed successfully!"), BadRequest);
    }

    /// <summary>
    /// Cancel a booking.
    /// </summary>
    /// <param name="id">The booking id.</param>
    /// <returns>The result of the cancellation.</returns>
    /// <response code="200">The booking was cancelled.</response>
    /// <response code="400">The booking id is invalid.</response>
    [HttpPut(ApiRoutes.Booking.Cancel)]
    public async Task<IActionResult> CancelBooking(int id)
    {
        return await Result
            .Create(id)
            .Map(i => new CancelBookingCommand(i))
            .Bind(command => Mediator.Send(command))
            .Match(() => Ok("Booking cancelled successfully!"), BadRequest);
    }

    /// <summary>
    /// Checkout a booking.
    /// </summary>
    /// <param name="id">The booking id.</param>
    /// <returns>The result of the checkout.</returns>
    /// <response code="200">The booking was checked out.</response>
    /// <response code="400">The booking id is invalid.</response>
    [HttpPost(ApiRoutes.Booking.Checkout)]
    public async Task<IActionResult> CheckoutBooking(int id) =>
        await Result
            .Create(id)
            .Map(x => new CheckoutBookingCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Search bookings.
    /// </summary>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filters">The filters.</param>
    /// <param name="sorting">The sorting.</param>
    /// <returns>The paged list of bookings.</returns>
    /// <response code="200">The bookings were found.</response>
    /// <response code="400">The search request is invalid.</response>
    [HttpGet(ApiRoutes.Booking.Search)]
    public async Task<IActionResult> SearchBookings(
        string? filters,
        string? sorting,
        int page = 1,
        int pageSize = 10
    )
    {
        return await Result
            .Create(new SearchBookingQuery(page, pageSize, filters, sorting))
            .Bind(query => Mediator.Send(query))
            .Match(Ok, BadRequest);
    }
}
