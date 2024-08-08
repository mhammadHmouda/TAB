using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Cities.AddCity;

public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.PostOffice).NotEmpty();
    }
}
