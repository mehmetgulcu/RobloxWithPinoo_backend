using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobloxWithPinoo.Migrations
{
    /// <inheritdoc />
    public partial class DocCategoryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IndexNo",
                table: "DocCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IndexNo",
                table: "DocCategories");
        }
    }
}
