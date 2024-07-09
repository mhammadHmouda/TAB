using Microsoft.AspNetCore.Mvc;

namespace TAB.WebApi.Controllers;

[ApiController]
[Route("/home")]
public class HomeController : ControllerBase
{
    // <summary>
    // This is a simple GET endpoint that returns a string.
    // </summary>
    // <param name="throwException">A boolean value that determines if an exception should be thrown.</param>
    // <returns>A string with the message "Hello World!"</returns>
    // <response code="200">Returns the message "Hello World!"</response>
    // <response code="500">Returns an error message if an exception is thrown.</response>
    [HttpGet]
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
    [HttpGet("welcome")]
    public IActionResult Welcome() =>
        Ok("Welcome to the Travel and Accommodation Booking Platform API!");
}
