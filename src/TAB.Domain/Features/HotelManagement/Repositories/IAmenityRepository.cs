﻿using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IAmenityRepository : IRepository<Amenity>
{
    Task<IEnumerable<Amenity>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken);
    Task<IEnumerable<Amenity>> GetByRoomIdAsync(int roomId, CancellationToken cancellationToken);
}
