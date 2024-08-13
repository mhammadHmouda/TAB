using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Core.Interfaces.Payment;

public interface IPaymentServiceFactory
{
    Result<IPaymentService> Create(string paymentMethod);
}
