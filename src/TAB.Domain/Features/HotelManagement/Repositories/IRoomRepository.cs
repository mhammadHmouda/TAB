using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IRoomRepository : IRepository<Room>
{
    Task<Maybe<Room>> GetByIdWithDiscountsAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Room>> SearchRoomsAsync(
        ISpecification<Room> spec,
        CancellationToken cancellationToken
    );
}
