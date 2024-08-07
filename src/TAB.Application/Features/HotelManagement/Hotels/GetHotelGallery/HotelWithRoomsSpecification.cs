using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelGallery;

public class HotelWithRoomsSpecification : BaseSpecification<Hotel>
{
    public HotelWithRoomsSpecification(int hotelId)
    {
        ApplyNoTracking();

        AddCriteria(h => h.Id == hotelId);
        AddInclude(h => h.Rooms);
    }
}
