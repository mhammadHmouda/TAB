using Microsoft.AspNetCore.Mvc;
using TAB.WebApi.Contracts;
using TAB.WebApi.Infrastructure;

namespace TAB.WebApi.Controllers;

public class HomeController : ApiController
{
    // <summary>
    // This is a simple GET endpoint that returns a string.
    // </summary>
    // <param name="throwException">A boolean value that determines if an exception should be thrown.</param>
    // <returns>A string with the message "Hello World!"</returns>
    // <response code="200">Returns the message "Hello World!"</response>
    // <response code="500">Returns an error message if an exception is thrown.</response>
    [HttpGet(ApiRoutes.Hello.Get)]
    public IActionResult Hello(bool throwException = false)
    {
        if (throwException)
        {
            throw new Exception("This is an exception!");
        }

        return Ok("Hello World!");
    }

    // <summary>
    // This is a simple GET endpoint that returns a string.
    // </summary>
    // <returns>A string with the message "Welcome to the Travel and Accommodation Booking Platform API!"</returns>
    // <response code="200">Returns the message "Welcome to the Travel and Accommodation Booking Platform API!"</response>
    [HttpGet(ApiRoutes.Hello.GetWithWelcome)]
    public IActionResult Welcome() =>
        Ok("Welcome to the Travel and Accommodation Booking Platform API!");
}
