using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Core.Enums;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Persistence.Configurations;

public class ImageEntityConfiguration : IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable(nameof(Image));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url).HasMaxLength(500).IsRequired();

        builder
            .Property(x => x.Type)
            .HasConversion(x => x.ToString(), x => (ImageType)Enum.Parse(typeof(ImageType), x));

        builder.Property(x => x.ReferenceId).IsRequired();

        builder.Property(x => x.CreatedAtUtc).IsRequired();

        builder.Property(x => x.UpdatedAtUtc);
    }
}
