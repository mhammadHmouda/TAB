using FluentValidation;
using TAB.Application.Core.Extensions;
using TAB.Domain.Core.Errors;

namespace TAB.Application.Features.HotelManagement.Amenities.AddAmenity;

public class CreateAmenityCommandValidator : AbstractValidator<CreateAmenityCommand>
{
    public CreateAmenityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithError(DomainErrors.Amenity.NameIsRequired);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(DomainErrors.Amenity.DescriptionIsRequired);

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithError(DomainErrors.Amenity.TypeShouldBeOneOfTheFollowingHotelRoom);

        RuleFor(x => x.TypeId)
            .NotEmpty()
            .WithError(DomainErrors.Amenity.TypeShouldBeOneOfTheFollowingHotelRoom);
    }
}
