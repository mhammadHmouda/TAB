using FluentValidation;

namespace TAB.Application.Features.BookingManagement.AddBooking;

public class BookingRoomCommandValidator : AbstractValidator<BookingRoomCommand>
{
    public BookingRoomCommandValidator()
    {
        RuleFor(x => x.CheckInDate).NotEmpty().WithMessage("The check in date is required.");
        RuleFor(x => x.CheckOutDate).NotEmpty().WithMessage("The check out date is required.");
        RuleFor(x => x.RoomId).NotEmpty().WithMessage("The room id is required.");
    }
}
