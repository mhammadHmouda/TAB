using MediatR;

namespace TAB.Application.Core.Contracts;

public interface IQuery<out TResponse> : IRequest<TResponse> { }
