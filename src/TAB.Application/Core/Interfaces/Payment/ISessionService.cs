using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Core.Interfaces.Payment;

public interface ISessionService
{
    Task<Result<Session>> SaveAsync(int bookingId, CancellationToken cancellation);
}
