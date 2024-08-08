using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TAB.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Booking");
        }
    }
}
