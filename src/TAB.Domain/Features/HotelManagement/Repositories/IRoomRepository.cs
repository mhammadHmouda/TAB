using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IRoomRepository : IRepository<Room>
{
    Task<Maybe<Room>> GetByIdWithDiscountsAsync(int id, CancellationToken cancellationToken);
}
