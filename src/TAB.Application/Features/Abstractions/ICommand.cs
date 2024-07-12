using MediatR;

namespace TAB.Application.Features.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse> { }
