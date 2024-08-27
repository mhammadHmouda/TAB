using FluentValidation;
using TAB.Application.Core.Contracts;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelGallery;

public class GetHotelGalleryQueryValidator : QueryPaginationValidator<GetHotelGalleryQuery>
{
    public GetHotelGalleryQueryValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("The hotel id is required.");
    }
}
