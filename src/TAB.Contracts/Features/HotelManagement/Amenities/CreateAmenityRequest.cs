namespace TAB.Contracts.Features.HotelManagement.Amenities;

public record CreateAmenityRequest(string Name, string Description, int TypeId);
