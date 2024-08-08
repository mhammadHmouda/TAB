using MediatR;

namespace TAB.Application.Core.Contracts;

public interface ICommand<out TResponse> : IRequest<TResponse> { }
