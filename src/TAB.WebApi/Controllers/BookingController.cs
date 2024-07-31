using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.BookingManagement.AddBooking;
using TAB.Application.Features.BookingManagement.ConfirmBooking;
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
}
