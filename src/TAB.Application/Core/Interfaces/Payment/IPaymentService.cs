using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Core.Interfaces.Payment;

public interface IPaymentService
{
    Task<Result<Session>> CreateSessionAsync(int bookingId, CancellationToken cancellation);
    PaymentMethod GetPaymentMethod();
}
