using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.HotelManagement;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    public ImageRepository(IDbContext context)
        : base(context) { }
}
