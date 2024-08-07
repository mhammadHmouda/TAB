using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Core.Interfaces.Payment;

public interface IPaymentService
{
    Task<Result<Session>> CreateStripeSessionAsync(int bookingId, CancellationToken cancellation);
    Task<Result<Session>> CreatePayPalSessionAsync(int bookingId, CancellationToken cancellation);
}
