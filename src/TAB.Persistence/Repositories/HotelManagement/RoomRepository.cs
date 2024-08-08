using Microsoft.EntityFrameworkCore;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.HotelManagement;

public class RoomRepository : BaseRepository<Room>, IRoomRepository
{
    public RoomRepository(IDbContext dbContext)
        : base(dbContext) { }

    public async Task<Maybe<Room>> GetByIdWithDiscountsAsync(
        int id,
        CancellationToken cancellationToken
    )
    {
        return await DbContext
                .Set<Room>()
                .Include(x => x.Discounts)
                .SingleOrDefaultAsync(x => x.Id == id, cancellationToken) ?? Maybe<Room>.None;
    }

    public async Task<IEnumerable<Room>> SearchRoomsAsync(
        ISpecification<Room> spec,
        CancellationToken cancellationToken
    ) => await GetAllAsync(spec, cancellationToken);
}
