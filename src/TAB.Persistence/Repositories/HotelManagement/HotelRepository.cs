using Microsoft.EntityFrameworkCore;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;
using TAB.Persistence.Specifications.HotelManagement;

namespace TAB.Persistence.Repositories.HotelManagement;

public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
{
    public HotelRepository(IDbContext dbContext)
        : base(dbContext) { }

    public async Task<Maybe<Hotel>> GetByIdWithReviewsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext
                .Set<Hotel>()
                .Include(h => h.Reviews)
                .SingleOrDefaultAsync(h => h.Id == id, cancellationToken) ?? Maybe<Hotel>.None;
    }

    public async Task<IReadOnlyCollection<Hotel>> SearchHotelsAsync(
        string? filters,
        string? sorting,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        var spec = new HotelsSearchSpecification(filters, sorting, page, pageSize);

        return await ApplySpecification(spec).ToListAsync(cancellationToken);
    }
}
