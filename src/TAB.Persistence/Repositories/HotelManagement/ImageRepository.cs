using Microsoft.EntityFrameworkCore;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Enums;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.HotelManagement;

public class ImageRepository : BaseRepository<Image>, IImageRepository
{
    public ImageRepository(IDbContext context)
        : base(context) { }

    public async Task<IEnumerable<Image>> GetByHotelIdAsync(
        int hotelId,
        CancellationToken cancellationToken = default
    )
    {
        return await GetAllAsync(
            i => i.ReferenceId == hotelId && i.Type == ImageType.Hotel,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Image>> GetByRoomIdAsync(
        int roomId,
        CancellationToken cancellationToken = default
    )
    {
        return await GetAllAsync(
            i => i.ReferenceId == roomId && i.Type == ImageType.Room,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Image>> GetByCityIdAsync(
        int cityId,
        CancellationToken cancellationToken = default
    )
    {
        return await GetAllAsync(
            i => i.ReferenceId == cityId && i.Type == ImageType.City,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Image>> GetAllByCityIdsAsync(
        List<int> cityIds,
        CancellationToken cancellationToken = default
    )
    {
        return await GetAllAsync(
            i => cityIds.Contains(i.ReferenceId) && i.Type == ImageType.City,
            cancellationToken
        );
    }

    public async Task<IEnumerable<Image>> GetRoomImagesAsync(
        List<int> galleryIds,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    )
    {
        return await DbContext
            .Set<Image>()
            .Where(i => galleryIds.Contains(i.ReferenceId) && i.Type == ImageType.Room)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }
}
