using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class ProductIndexName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c8c75510-f428-40fa-bb9c-88557e4926ad", "AQAAAAIAAYagAAAAEOeI9ITVe46SXYc5tWWCKftzkL79uYdD9RZgOurPLIxDfSY4wlhXbQ1Vg6iVuBVELQ==", "69ccdb33-ce8b-4b7f-ad30-e721c4c88edb" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductName",
                table: "Products",
                column: "ProductName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_ProductName",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a369dcae-6b8e-4a44-84c2-d3d72c9cf7c6", "AQAAAAIAAYagAAAAEOlYwkg7mT/dqkAjLOOvIcO2PcJ4WVL7LLi6b5YhY4J0NBNMi6fHnA6frVJkF9E7HQ==", "c737ea21-7bba-485c-8011-8ee035d06776" });
        }
    }
}
