﻿using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IAmenityRepository : IRepository<Amenity>
{
    Task<List<Amenity>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken);
}
