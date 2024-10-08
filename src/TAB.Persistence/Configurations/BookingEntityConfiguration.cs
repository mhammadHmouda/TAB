﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Enums;

namespace TAB.Persistence.Configurations;

public class BookingEntityConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable(nameof(Booking));

        builder.HasKey(b => b.Id);

        builder.Property(b => b.CheckInDate).IsRequired();

        builder.Property(b => b.CheckOutDate).IsRequired();

        builder.OwnsOne(
            room => room.TotalPrice,
            priceBuilder =>
            {
                priceBuilder.WithOwner();

                priceBuilder
                    .Property(x => x.Amount)
                    .HasColumnName("Price")
                    .IsRequired()
                    .HasPrecision(18, 2)
                    .HasColumnType("decimal(18,2)");

                priceBuilder
                    .Property(x => x.Currency)
                    .HasColumnName("Currency")
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasColumnType("char(3)");
            }
        );

        builder.Property(b => b.UserId).IsRequired();

        builder.Property(b => b.HotelId).IsRequired();

        builder.Property(b => b.RoomId).IsRequired();

        builder.Property(x => x.SessionId).IsRequired(false);

        builder
            .Property(x => x.Status)
            .HasConversion(
                v => v.ToString(),
                v => (BookingStatus)Enum.Parse(typeof(BookingStatus), v)
            );
        builder.Property(b => b.CreatedAtUtc).IsRequired();

        builder.Property(b => b.UpdatedAtUtc);

        builder
            .HasOne(x => x.Hotel)
            .WithMany()
            .HasForeignKey(x => x.HotelId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
