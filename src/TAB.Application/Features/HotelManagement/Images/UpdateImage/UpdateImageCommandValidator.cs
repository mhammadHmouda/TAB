using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Images.UpdateImage;

public class UpdateImageCommandValidator : AbstractValidator<UpdateImageCommand>
{
    public UpdateImageCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.File).NotNull();
    }
}
