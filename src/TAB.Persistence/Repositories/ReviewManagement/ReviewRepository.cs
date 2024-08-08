using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.ReviewManagement.Entities;
using TAB.Domain.Features.ReviewManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.ReviewManagement;

public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(IDbContext dbContext)
        : base(dbContext) { }
}
