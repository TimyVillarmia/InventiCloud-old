using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class StockTransferRevision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "StockTransfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_CreatedById",
                table: "StockTransfers",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_AspNetUsers_CreatedById",
                table: "StockTransfers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_AspNetUsers_CreatedById",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_CreatedById",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "StockTransfers");
        }
    }
}
