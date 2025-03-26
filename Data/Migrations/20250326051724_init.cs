using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Branches_BranchID",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductID",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Inventories",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "BranchID",
                table: "Inventories",
                newName: "BranchId");

            migrationBuilder.RenameColumn(
                name: "InventroyId",
                table: "Inventories",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_ProductID",
                table: "Inventories",
                newName: "IX_Inventories_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_BranchID",
                table: "Inventories",
                newName: "IX_Inventories_BranchId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7568dc33-76be-492e-b154-f3bd44261390", "AQAAAAIAAYagAAAAEAEDguWDs8iOEwuuIrVS/f65un7ejYA8Ff0jGb62eK1oL1SisBm3wNIAmWOTYlgm0g==", "63d487e8-e5c0-4a64-ae49-7bef0a80220b" });

            migrationBuilder.InsertData(
                table: "Inventories",
                columns: new[] { "InventoryId", "AvailableQuantity", "BranchId", "IncomingQuantity", "OnHandquantity", "OutgoingQuantity", "ProductId" },
                values: new object[,]
                {
                    { 1, 0, 1, 0, 100, 0, 1 },
                    { 2, 0, 2, 0, 50, 0, 1 },
                    { 3, 0, 3, 0, 75, 0, 1 },
                    { 4, 0, 1, 0, 200, 0, 2 },
                    { 5, 0, 2, 0, 150, 0, 2 },
                    { 6, 0, 3, 0, 150, 0, 2 },
                    { 7, 0, 1, 0, 80, 0, 3 },
                    { 8, 0, 2, 0, 120, 0, 3 },
                    { 9, 0, 3, 0, 90, 0, 3 },
                    { 10, 0, 1, 0, 300, 0, 4 },
                    { 11, 0, 2, 0, 250, 0, 4 },
                    { 12, 0, 3, 0, 280, 0, 4 },
                    { 13, 0, 1, 0, 60, 0, 5 },
                    { 14, 0, 2, 0, 40, 0, 5 },
                    { 15, 0, 3, 0, 70, 0, 5 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Branches_BranchId",
                table: "Inventories",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Branches_BranchId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Products_ProductId",
                table: "Inventories");

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Inventories",
                keyColumn: "InventoryId",
                keyValue: 15);

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Inventories",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Inventories",
                newName: "BranchID");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "Inventories",
                newName: "InventroyId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_ProductId",
                table: "Inventories",
                newName: "IX_Inventories_ProductID");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_BranchId",
                table: "Inventories",
                newName: "IX_Inventories_BranchID");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54a73911-995a-46ea-aa35-4ab93472178e", "AQAAAAIAAYagAAAAEF3RDIW7lh+vTP9hXKvfrVBEgLGZ94IQuy5Dyu0qHTRfdjJIjQjRI9HyWxdeZCXuGg==", "73580bc3-9879-4f43-ad0c-989ab5ce9d09" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Branches_BranchID",
                table: "Inventories",
                column: "BranchID",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Products_ProductID",
                table: "Inventories",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
