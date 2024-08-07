using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelGallery;

public record GetHotelGalleryQuery(int Id, int Page, int PageSize)
    : IQuery<Result<ImageResponse[]>>;
