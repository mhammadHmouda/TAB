using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.BookingManagement.SuccessPayment;

public record SuccessPaymentCommand(string SessionId) : ICommand<Result>;
