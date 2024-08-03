using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels.GetFeaturedDeals;

public class GetFeaturedDealsSpecification : BaseSpecification<Hotel>
{
    public GetFeaturedDealsSpecification()
    {
        ApplyNoTracking();

        AddInclude(hotel => hotel.Rooms);
        AddInclude($"{nameof(Hotel.Rooms)}.{nameof(Room.Discounts)}");

        AddCriteria(hotel => hotel.Rooms.Any(r => r.IsAvailable));
    }
}
