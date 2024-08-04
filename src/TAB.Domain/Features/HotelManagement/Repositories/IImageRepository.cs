using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Repositories;

public interface IImageRepository : IRepository<Image>
{
    Task<IEnumerable<Image>> GetByHotelIdAsync(int hotelId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetByRoomIdAsync(int roomId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetByCityIdAsync(int cityId, CancellationToken cancellationToken);
    Task<IEnumerable<Image>> GetAllByCityIdsAsync(
        List<int> cityIds,
        CancellationToken cancellationToken
    );
    Task<IEnumerable<Image>> GetRoomImagesAsync(
        List<int> galleryIds,
        int page,
        int pageSize,
        CancellationToken cancellationToken
    );
}
