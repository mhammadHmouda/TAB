using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Discounts.AddDiscount;
using TAB.Application.Features.HotelManagement.Rooms.DeleteRoom;
using TAB.Application.Features.HotelManagement.Rooms.UpdateRoom;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing rooms.
/// </summary>
[TokenValidation]
[Authorize(Roles = "Admin")]
public class RoomController : ApiController
{
    /// <summary>
    /// Adds a discount to a room.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="request">The request containing the discount details.</param>
    /// <response code="200">The discount was added successfully.</response>
    /// <response code="400">The discount was not added successfully.</response>
    /// <returns>The result of the add discount operation.</returns>
    [HttpPost(ApiRoutes.Rooms.AddDiscount)]
    public async Task<IActionResult> AddDiscount(int id, [FromBody] CreateDiscountRequest request)
    {
        return await Result
            .Create((id, command: request))
            .Ensure(x => x.command.RoomId == x.id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new CreateDiscountCommand(
                x.command.RoomId,
                x.command.Name,
                x.command.Description,
                x.command.DiscountPercentage,
                x.command.StartDate,
                x.command.EndDate
            ))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
    }

    /// <summary>
    /// Updates a room.
    /// </summary>
    /// <param name="id">The ID of the room.</param>
    /// <param name="request">The request containing the updated room details.</param>
    /// <response code="200">The room was updated successfully.</response>
    /// <response code="400">The room was not updated successfully.</response>
    /// <returns>The result of the update operation.</returns>
    [HttpPut(ApiRoutes.Rooms.Update)]
    public async Task<IActionResult> Update(int id, UpdateRoomRequest request) =>
        await Result
            .Create((id, request))
            .Ensure(x => x.id == x.request.Id, DomainErrors.General.UnProcessableRequest)
            .Map(x => new UpdateRoomCommand(
                x.id,
                x.request.Number,
                x.request.Price,
                x.request.Currency,
                x.request.Type,
                x.request.CapacityOfAdults,
                x.request.CapacityOfChildren
            ))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);

    /// <summary>
    /// Deletes a room.
    /// </summary>
    /// <param name="id">The ID of the room to delete.</param>
    /// <response code="200">The room was deleted successfully.</response>
    /// <response code="400">The room was not deleted successfully.</response>
    /// <returns>The result of the delete operation.</returns>
    [HttpDelete(ApiRoutes.Rooms.Delete)]
    public async Task<IActionResult> Delete(int id) =>
        await Result
            .Create(id)
            .Map(x => new DeleteRoomCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(Ok, BadRequest);
}
