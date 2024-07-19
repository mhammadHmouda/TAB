using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.HotelManagement;

public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
{
    public HotelRepository(IDbContext dbContext)
        : base(dbContext) { }
}
