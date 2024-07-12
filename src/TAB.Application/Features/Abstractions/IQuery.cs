using MediatR;

namespace TAB.Application.Features.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
