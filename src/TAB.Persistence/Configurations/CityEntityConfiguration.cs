using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Persistence.Configurations;

public class CityEntityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(city => city.Id);

        builder.Property(city => city.Name).HasMaxLength(100).IsRequired();

        builder.Property(city => city.Country).HasMaxLength(100).IsRequired();

        builder.Property(city => city.PostOffice).HasMaxLength(100).IsRequired();

        builder.Property(city => city.CreatedAtUtc).IsRequired();

        builder.Property(city => city.UpdatedAtUtc).IsRequired(false);
    }
}
