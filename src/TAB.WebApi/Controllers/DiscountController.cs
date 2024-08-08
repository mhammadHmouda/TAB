using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.HotelManagement.Discounts.DeleteDiscount;
using TAB.Application.Features.HotelManagement.Discounts.SearchDiscounts;
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

    /// <summary>
    /// Searches for hotel discounts.
    /// </summary>
    /// <param name="hotelId">The ID of the hotel.</param>
    /// <param name="page">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="sorting">The sorting to apply.</param>
    /// <response code="200">The discounts were found successfully.</response>
    /// <response code="400">The discounts were not found successfully.</response>
    /// <returns>The result of the search discount operation.</returns>
    [HttpGet(ApiRoutes.Discounts.Search)]
    public async Task<IActionResult> Search(
        int hotelId,
        string? filters,
        string? sorting,
        int page = 1,
        int pageSize = 10
    )
    {
        return await Result
            .Create(new SearchDiscountQuery(hotelId, page, pageSize, filters, sorting))
            .Bind(q => Mediator.Send(q))
            .Match(Ok, BadRequest);
    }
}
