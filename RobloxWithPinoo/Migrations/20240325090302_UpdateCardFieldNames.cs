using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobloxWithPinoo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCardFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Socket2",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Socket3",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Socket4",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "Socket9",
                table: "Cards",
                newName: "Pinoo9");

            migrationBuilder.RenameColumn(
                name: "Socket8",
                table: "Cards",
                newName: "Pinoo8");

            migrationBuilder.RenameColumn(
                name: "Socket7",
                table: "Cards",
                newName: "Pinoo7");

            migrationBuilder.RenameColumn(
                name: "Socket6",
                table: "Cards",
                newName: "Pinoo6");

            migrationBuilder.RenameColumn(
                name: "Socket5",
                table: "Cards",
                newName: "Pinoo5");

            migrationBuilder.RenameColumn(
                name: "Socket10",
                table: "Cards",
                newName: "Pinoo10");

            migrationBuilder.AddColumn<int>(
                name: "Pinoo2",
                table: "Cards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Pinoo3",
                table: "Cards",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Pinoo4",
                table: "Cards",
                type: "real",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pinoo2",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Pinoo3",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Pinoo4",
                table: "Cards");

            migrationBuilder.RenameColumn(
                name: "Pinoo9",
                table: "Cards",
                newName: "Socket9");

            migrationBuilder.RenameColumn(
                name: "Pinoo8",
                table: "Cards",
                newName: "Socket8");

            migrationBuilder.RenameColumn(
                name: "Pinoo7",
                table: "Cards",
                newName: "Socket7");

            migrationBuilder.RenameColumn(
                name: "Pinoo6",
                table: "Cards",
                newName: "Socket6");

            migrationBuilder.RenameColumn(
                name: "Pinoo5",
                table: "Cards",
                newName: "Socket5");

            migrationBuilder.RenameColumn(
                name: "Pinoo10",
                table: "Cards",
                newName: "Socket10");

            migrationBuilder.AddColumn<bool>(
                name: "Socket2",
                table: "Cards",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Socket3",
                table: "Cards",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Socket4",
                table: "Cards",
                type: "float",
                nullable: true);
        }
    }
}
