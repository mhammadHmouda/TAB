using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Persistence.Configurations;

public class HotelEntityConfiguration : IEntityTypeConfiguration<Hotel>
{
    public void Configure(EntityTypeBuilder<Hotel> builder)
    {
        builder.ToTable(nameof(Hotel));

        builder.HasKey(hotel => hotel.Id);

        builder.Property(hotel => hotel.Name).HasMaxLength(100).IsRequired();

        builder.Property(hotel => hotel.Description).HasMaxLength(500).IsRequired();

        builder.Property(hotel => hotel.StarRating).IsRequired();

        builder.Property(hotel => hotel.RoomsNumber).IsRequired();

        builder.Property(hotel => hotel.Type).IsRequired();

        builder.OwnsOne(
            hotel => hotel.Location,
            locationBuilder =>
            {
                locationBuilder.WithOwner();

                locationBuilder.Property(x => x.Latitude).HasColumnName("Latitude").IsRequired();
                locationBuilder.Property(x => x.Longitude).HasColumnName("Longitude").IsRequired();
            }
        );

        builder.Property(hotel => hotel.CreatedAtUtc).IsRequired();
        builder.Property(hotel => hotel.UpdatedAtUtc).IsRequired(false);

        builder
            .HasOne(hotel => hotel.City)
            .WithMany()
            .HasForeignKey(hotel => hotel.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(hotel => hotel.Owner)
            .WithMany()
            .HasForeignKey(hotel => hotel.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(hotel => hotel.Rooms)
            .WithOne()
            .HasForeignKey(room => room.HotelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
