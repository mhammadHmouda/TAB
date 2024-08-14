using TAB.Application.Core.Interfaces.Payment;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Infrastructure.Payment;

public class PaymentServiceFactory : IPaymentServiceFactory
{
    private readonly IDictionary<string, IPaymentService> _paymentServices;

    public PaymentServiceFactory(IEnumerable<IPaymentService> paymentServices)
    {
        _paymentServices = paymentServices.ToDictionary(service =>
            service.GetPaymentMethod().ToString().ToLower()
        );
    }

    public Result<IPaymentService> Create(string paymentMethod)
    {
        _paymentServices.TryGetValue(paymentMethod.ToLower(), out var paymentService);

        return paymentService != null
            ? Result<IPaymentService>.Success(paymentService)
            : DomainErrors.General.InvalidPaymentMethod;
    }
}
