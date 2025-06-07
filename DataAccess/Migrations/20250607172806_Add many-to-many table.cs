using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addmanytomanytable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LootBoxDigitalItem_DigitalItems_DigitalItemId",
                table: "LootBoxDigitalItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LootBoxDigitalItem_LootBoxes_LootBoxId",
                table: "LootBoxDigitalItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LootBoxDigitalItem",
                table: "LootBoxDigitalItem");

            migrationBuilder.RenameTable(
                name: "LootBoxDigitalItem",
                newName: "LootBoxDigitalItems");

            migrationBuilder.RenameIndex(
                name: "IX_LootBoxDigitalItem_DigitalItemId",
                table: "LootBoxDigitalItems",
                newName: "IX_LootBoxDigitalItems_DigitalItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LootBoxDigitalItems",
                table: "LootBoxDigitalItems",
                columns: new[] { "LootBoxId", "DigitalItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LootBoxDigitalItems_DigitalItems_DigitalItemId",
                table: "LootBoxDigitalItems",
                column: "DigitalItemId",
                principalTable: "DigitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LootBoxDigitalItems_LootBoxes_LootBoxId",
                table: "LootBoxDigitalItems",
                column: "LootBoxId",
                principalTable: "LootBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LootBoxDigitalItems_DigitalItems_DigitalItemId",
                table: "LootBoxDigitalItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LootBoxDigitalItems_LootBoxes_LootBoxId",
                table: "LootBoxDigitalItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LootBoxDigitalItems",
                table: "LootBoxDigitalItems");

            migrationBuilder.RenameTable(
                name: "LootBoxDigitalItems",
                newName: "LootBoxDigitalItem");

            migrationBuilder.RenameIndex(
                name: "IX_LootBoxDigitalItems_DigitalItemId",
                table: "LootBoxDigitalItem",
                newName: "IX_LootBoxDigitalItem_DigitalItemId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LootBoxDigitalItem",
                table: "LootBoxDigitalItem",
                columns: new[] { "LootBoxId", "DigitalItemId" });

            migrationBuilder.AddForeignKey(
                name: "FK_LootBoxDigitalItem_DigitalItems_DigitalItemId",
                table: "LootBoxDigitalItem",
                column: "DigitalItemId",
                principalTable: "DigitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LootBoxDigitalItem_LootBoxes_LootBoxId",
                table: "LootBoxDigitalItem",
                column: "LootBoxId",
                principalTable: "LootBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
