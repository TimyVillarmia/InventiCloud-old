using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class DefaultBranchandPOStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Branches",
                columns: new[] { "BranchId", "Address", "BranchName", "City", "Country", "Email", "PhoneNumber", "PostalCode", "Region" },
                values: new object[,]
                {
                    { 1, "123 Main St", "Main Warehouse", "Anytown", "USA", "warehouse@example.com", "555-123-4567", "12345", "State" },
                    { 2, "456 Oak Ave", "Retail Store A", "Springfield", "Canada", "retailA@example.com", "123-456-7890", "A1B 2C3", "Province" },
                    { 3, "789 Pine Ln", "Distribution Center", "London", "UK", "distribution@example.com", "+44 20 1234 5678", "SW1A 1AA", "England" }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrderStatuses",
                columns: new[] { "PurchaseOrderStatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Ordered" },
                    { 3, "Completed" },
                    { 4, "Cancelled" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PurchaseOrderStatuses",
                keyColumn: "PurchaseOrderStatusId",
                keyValue: 4);
        }
    }
}
