using TAB.Application.Core.Contracts;
using TAB.Application.Core.Extensions;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Result;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TAB.Application.Features.BookingManagement.CheckoutRoom;

public record CheckoutBookingCommand(int BookingId) : ICommand<Result<Session>>;
