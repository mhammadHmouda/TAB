using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace TAB.Persistence.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Email)
            .HasConversion(x => x.Value, x => Email.Create(x).Value)
            .HasMaxLength(Email.MaxLength)
            .IsRequired();

        builder.Property(x => x.Password).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(100).IsRequired();
        builder.Property(x => x.Role).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();

        builder.OwnsOne(
            user => user.ActivationCode,
            activationCode =>
            {
                activationCode.WithOwner();

                activationCode.Property(x => x.Value).HasColumnName("ActivationCode");
                activationCode
                    .Property(x => x.ExpiresAtUtc)
                    .HasColumnName("ActivationCodeExpiresAt");
            }
        );

        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc);

        builder.HasIndex(x => x.Email).IsUnique();

        builder
            .HasMany(x => x.Tokens)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
