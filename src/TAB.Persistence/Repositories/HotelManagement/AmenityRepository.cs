using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.HotelManagement;

public class AmenityRepository : BaseRepository<Amenity>, IAmenityRepository
{
    public AmenityRepository(IDbContext context)
        : base(context) { }

    public async Task<IEnumerable<Amenity>> GetByHotelIdAsync(
        int hotelId,
        CancellationToken cancellationToken = default
    )
    {
        return await GetAllAsync(
            a => a.TypeId == hotelId && a.Type == AmenityType.Hotel,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Amenity>> GetByRoomIdAsync(
        int roomId,
        CancellationToken cancellationToken = default
    )
    {
        return await GetAllAsync(
            a => a.TypeId == roomId && a.Type == AmenityType.Room,
            cancellationToken
        );
    }
}
