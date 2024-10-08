﻿namespace TAB.Contracts.Features.HotelManagement.Discounts;

public record DiscountResponse(
    int Id,
    string Name,
    string Description,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    int RoomId
);
