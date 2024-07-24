namespace TAB.Contracts.Features.HotelManagement.Discounts;

public record CreateDiscountRequest(
    int RoomId,
    string Name,
    string Description,
    int DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate
);
