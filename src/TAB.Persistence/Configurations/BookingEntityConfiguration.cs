using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Persistence.Configurations;

public class BookingEntityConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CheckInDate).IsRequired();

        builder.Property(b => b.CheckOutDate).IsRequired();

        builder.Property(b => b.TotalPrice).IsRequired();

        builder.Property(b => b.UserId).IsRequired();

        builder.Property(b => b.HotelId).IsRequired();

        builder.Property(b => b.RoomId).IsRequired();

        builder.Property(b => b.Status).IsRequired();

        builder.Property(b => b.CreatedAtUtc).IsRequired();

        builder.Property(b => b.UpdatedAtUtc);

        builder
            .HasOne<Room>()
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Hotel>()
            .WithMany()
            .HasForeignKey(b => b.HotelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
