﻿using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IImageRepository : IRepository<Image>
{
    Task<IEnumerable<Image>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken);
}
