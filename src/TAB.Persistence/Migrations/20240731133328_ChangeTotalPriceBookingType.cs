using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAB.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTotalPriceBookingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Booking");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Booking",
                type: "char(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Booking",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Booking");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
