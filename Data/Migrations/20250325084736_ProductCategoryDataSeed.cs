using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategoryDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "54a73911-995a-46ea-aa35-4ab93472178e", "AQAAAAIAAYagAAAAEF3RDIW7lh+vTP9hXKvfrVBEgLGZ94IQuy5Dyu0qHTRfdjJIjQjRI9HyWxdeZCXuGg==", "73580bc3-9879-4f43-ad0c-989ab5ce9d09" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                column: "BranchName",
                value: "Branch A");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                column: "BranchName",
                value: "Branch B");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3,
                column: "BranchName",
                value: "Branch C");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Eyeglasses" },
                    { 2, "Contact Lenses" },
                    { 3, "Reading Glasses" },
                    { 4, "Eye Care Products" },
                    { 5, "Sunglasses" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "AttributeSetId", "Brand", "CategoryId", "Description", "ImageURL", "ProductName", "SKU", "UnitCost", "UnitPrice", "isActive" },
                values: new object[,]
                {
                    { 1, null, "VisionGuard", 1, "High-quality glasses to protect your eyes from harmful blue light.", "glasses_blue_light.jpg", "Premium Blue Light Blocking Glasses", "VG-BL-001", 50.00m, 120.00m, true },
                    { 2, null, "AquaView", 2, "Comfortable daily disposable contact lenses for clear vision.", "contact_lenses_daily.jpg", "Daily Disposable Contact Lenses", "AV-CD-002", 15.00m, 35.00m, true },
                    { 3, null, "ReadWell", 3, "Stylish reading glasses with anti-glare coating for reduced eye strain.", "reading_glasses_anti_glare.jpg", "Anti-Glare Reading Glasses", "RW-RG-003", 25.00m, 60.00m, true },
                    { 4, null, "MoisturePlus", 4, "Relief from dry, irritated eyes with these lubricating eye drops.", "eye_drops_dry_eyes.jpg", "Eye Drops for Dry Eyes", "MP-ED-004", 8.00m, 20.00m, true },
                    { 5, null, "SunStyle", 1, "Fashionable sunglasses with UV protection for sunny days.", "designer_sunglasses.jpg", "Designer Sunglasses", "SS-SG-005", 80.00m, 200.00m, true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8c75510-f428-40fa-bb9c-88557e4926ad", "AQAAAAIAAYagAAAAEOeI9ITVe46SXYc5tWWCKftzkL79uYdD9RZgOurPLIxDfSY4wlhXbQ1Vg6iVuBVELQ==", "69ccdb33-ce8b-4b7f-ad30-e721c4c88edb" });

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 1,
                column: "BranchName",
                value: "Main Warehouse");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 2,
                column: "BranchName",
                value: "Retail Store A");

            migrationBuilder.UpdateData(
                table: "Branches",
                keyColumn: "BranchId",
                keyValue: 3,
                column: "BranchName",
                value: "Distribution Center");
        }
    }
}
