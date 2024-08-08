using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Discounts.DeleteDiscount;

public record DeleteDiscountCommand(int DiscountId) : ICommand<Result>;
