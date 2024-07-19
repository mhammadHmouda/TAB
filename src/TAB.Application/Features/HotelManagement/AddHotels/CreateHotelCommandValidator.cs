using FluentValidation;

namespace TAB.Application.Features.HotelManagement.AddHotels;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The hotel name is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("The hotel description is required.");
        RuleFor(x => x.Latitude).NotEmpty().WithMessage("Latitude is required.");
        RuleFor(x => x.Longitude).NotEmpty().WithMessage("Longitude is required.");
        RuleFor(x => x.CityId).NotEqual(0).WithMessage("The city id is required.");
        RuleFor(x => x.OwnerId).NotEqual(0).WithMessage("The owner id is required.");
        RuleFor(x => x.Type).IsInEnum().WithMessage("The hotel type is required.");
    }
}
