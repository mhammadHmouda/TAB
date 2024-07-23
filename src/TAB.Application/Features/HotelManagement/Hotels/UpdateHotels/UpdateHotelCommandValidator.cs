using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Hotels.UpdateHotels;

public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
{
    public UpdateHotelCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
        RuleFor(x => x.Latitude).NotEmpty().WithMessage("Latitude is required.");
        RuleFor(x => x.Longitude).NotEmpty().WithMessage("Longitude is required.");
    }
}
