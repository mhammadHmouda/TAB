using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.HotelManagement;

public class DiscountRepository : BaseRepository<Discount>, IDiscountRepository
{
    public DiscountRepository(IDbContext dbContext)
        : base(dbContext) { }
}
