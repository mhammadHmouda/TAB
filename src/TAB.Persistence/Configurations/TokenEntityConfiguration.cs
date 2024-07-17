using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Persistence.Configurations;

public class TokenEntityConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable(nameof(Token));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Value).HasMaxLength(100).IsRequired();
        builder.Property(x => x.IsRevoked).IsRequired();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();

        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.Value).IsUnique();

        builder
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
