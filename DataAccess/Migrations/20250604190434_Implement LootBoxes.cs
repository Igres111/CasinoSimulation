using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ImplementLootBoxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LootBoxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LootBoxes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LootBoxDigitalItem",
                columns: table => new
                {
                    LootBoxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DigitalItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LootBoxDigitalItem", x => new { x.LootBoxId, x.DigitalItemId });
                    table.ForeignKey(
                        name: "FK_LootBoxDigitalItem_DigitalItems_DigitalItemId",
                        column: x => x.DigitalItemId,
                        principalTable: "DigitalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LootBoxDigitalItem_LootBoxes_LootBoxId",
                        column: x => x.LootBoxId,
                        principalTable: "LootBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LootBoxDigitalItem_DigitalItemId",
                table: "LootBoxDigitalItem",
                column: "DigitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LootBoxes_UserId",
                table: "LootBoxes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LootBoxDigitalItem");

            migrationBuilder.DropTable(
                name: "LootBoxes");
        }
    }
}
