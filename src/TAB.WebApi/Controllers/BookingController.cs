using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.BookingManagement.AddBooking;
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
}
