using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<Maybe<Hotel>> GetByIdWithReviewsAsync(int hotelId, CancellationToken cancellationToken);
    Task<IEnumerable<Hotel>> SearchHotelsAsync(
        ISpecification<Hotel> spec,
        CancellationToken cancellationToken
    );
}
