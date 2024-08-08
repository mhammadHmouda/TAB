using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelById;

public class HotelSearchSpecification : BaseSpecification<Hotel>
{
    public HotelSearchSpecification(int hotelId)
    {
        AddCriteria(hotel => hotel.Id == hotelId);

        AddInclude(hotel => hotel.City);
        AddInclude(hotel => hotel.Owner);
        AddInclude(hotel => hotel.Rooms);
        AddInclude($"{nameof(Hotel.Rooms)}.{nameof(Room.Discounts)}");
    }
}
