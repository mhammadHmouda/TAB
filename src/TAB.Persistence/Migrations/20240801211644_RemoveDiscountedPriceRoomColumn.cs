using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAB.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDiscountedPriceRoomColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "Room");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedPrice",
                table: "Room",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
