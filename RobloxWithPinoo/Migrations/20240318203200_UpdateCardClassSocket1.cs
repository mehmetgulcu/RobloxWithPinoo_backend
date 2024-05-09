using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobloxWithPinoo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardClassSocket1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Socket1",
                table: "Cards");

            migrationBuilder.AddColumn<int>(
                name: "Pinoo1",
                table: "Cards",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pinoo1",
                table: "Cards");

            migrationBuilder.AddColumn<bool>(
                name: "Socket1",
                table: "Cards",
                type: "bit",
                nullable: true);
        }
    }
}
