using System.Net;
using TAB.Domain.Core.Shared;

namespace TAB.WebApi.Contracts;

public class ApiResponse<TValue, TError>
{
    public TValue? Data { get; set; } = default;
    public IReadOnlyCollection<TError>? Errors { get; set; } = default;
    public string? Message { get; set; }
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
}

public class ApiResponse<TValue> : ApiResponse<TValue, Error> { }

public class ApiResponse : ApiResponse<object, Error> { }
