using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.BookingManagement;

public class BookingRepository : BaseRepository<Booking>, IBookingRepository
{
    public BookingRepository(IDbContext dbContext)
        : base(dbContext) { }
}
