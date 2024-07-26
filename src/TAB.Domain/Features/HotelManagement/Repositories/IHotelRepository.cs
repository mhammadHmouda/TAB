using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<Maybe<Hotel>> GetByIdWithReviewsAsync(int hotelId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Hotel>> SearchHotelsAsync(
        string? filters,
        string? sorting,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );
}
