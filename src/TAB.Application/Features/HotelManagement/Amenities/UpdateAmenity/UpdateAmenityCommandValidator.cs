using FluentValidation;
using TAB.Application.Core.Extensions;
using TAB.Domain.Core.Errors;

namespace TAB.Application.Features.HotelManagement.Amenities.UpdateAmenity;

public class UpdateAmenityCommandValidator : AbstractValidator<UpdateAmenityCommand>
{
    public UpdateAmenityCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithError(DomainErrors.Amenity.IdIsRequired);
        RuleFor(x => x.Name).NotEmpty().WithError(DomainErrors.Amenity.NameIsRequired);
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithError(DomainErrors.Amenity.DescriptionIsRequired);
    }
}
