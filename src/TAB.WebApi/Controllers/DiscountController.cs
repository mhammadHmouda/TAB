using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Discounts.DeleteDiscount;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Controller for managing discounts.
/// </summary>
[TokenValidation]
[Authorize(Roles = "Admin")]
public class DiscountController : ApiController
{
    /// <summary>
    /// Deletes a discount.
    /// </summary>
    /// <param name="id">The ID of the discount.</param>
    /// <response code="200">The discount was deleted successfully.</response>
    /// <response code="400">The discount was not deleted successfully.</response>
    /// <returns>The result of the delete discount operation.</returns>
    [HttpDelete(ApiRoutes.Discounts.Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        return await Result
            .Create(id)
            .Map(i => new DeleteDiscountCommand(i))
            .Bind(c => Mediator.Send(c))
            .Match(() => Ok("Discount deleted successfully!"), BadRequest);
    }
}
