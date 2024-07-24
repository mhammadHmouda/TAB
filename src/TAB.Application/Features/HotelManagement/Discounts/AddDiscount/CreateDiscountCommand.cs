using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Discounts.AddDiscount;

public record CreateDiscountCommand(
    int RoomId,
    string Name,
    string Description,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
) : ICommand<Result<DiscountResponse>>;
