using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RefactorUserDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DigitalItems_Users_UserId",
                table: "DigitalItems");

            migrationBuilder.DropIndex(
                name: "IX_DigitalItems_UserId",
                table: "DigitalItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DigitalItems");

            migrationBuilder.AddColumn<decimal>(
                name: "BonusPoints",
                table: "DigitalItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusPoints",
                table: "DigitalItems");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "DigitalItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DigitalItems_UserId",
                table: "DigitalItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DigitalItems_Users_UserId",
                table: "DigitalItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
