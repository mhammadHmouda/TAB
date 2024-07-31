using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Maybe;

namespace TAB.Application.Core.Interfaces.Payment;

public interface ISessionService
{
    Task<string> GetByIdAsync(string sessionId, CancellationToken cancellation = default);
    Task<Maybe<Session>> SaveAsync(int bookingId, CancellationToken cancellation);
}
