using Microsoft.AspNetCore.Mvc;
using TAB.Application.Features.BookingManagement.SuccessPayment;
using TAB.Domain.Core.Shared.Result;
using TAB.WebApi.Abstractions;
using TAB.WebApi.Attributes;
using TAB.WebApi.Contracts;

namespace TAB.WebApi.Controllers;

/// <summary>
/// Payment controller.
/// </summary>
[TokenValidation]
public class PaymentController : ApiController
{
    /// <summary>
    /// Payment success endpoint.
    /// </summary>
    /// <param name="sessionId">Session ID.</param>
    /// <returns>Success payment response.</returns>
    /// <response code="200">Success payment response.</response>
    /// <response code="400">Error response.</response>
    [HttpGet(ApiRoutes.Payment.Success)]
    public async Task<IActionResult> Success(string sessionId) =>
        await Result
            .Create(sessionId)
            .Map(x => new SuccessPaymentCommand(x))
            .Bind(x => Mediator.Send(x))
            .Match(() => Ok("The booking payed successfully!"), BadRequest);
}
