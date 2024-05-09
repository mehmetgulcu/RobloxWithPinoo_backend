using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RobloxWithPinoo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProjectClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Projects_ProjectId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ProjectId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Cards");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Cards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ProjectId",
                table: "Cards",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Projects_ProjectId",
                table: "Cards",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
