using FluentValidation;

namespace TAB.Application.Features.HotelManagement.Images.UploadImages;

public class UploadImagesCommandValidator : AbstractValidator<UploadImagesCommand>
{
    public UploadImagesCommandValidator()
    {
        RuleFor(x => x.ReferenceId).GreaterThan(0);
        RuleFor(x => x.ImageType).IsInEnum();
        RuleFor(x => x.Files).NotEmpty();
    }
}
