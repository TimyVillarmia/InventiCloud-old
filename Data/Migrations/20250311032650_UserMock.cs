using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class UserMock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "your-user-id-1", 0, "c247c5e7-0e47-4da7-a607-205fcb25110e", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEA3LpOvTJs9/lcCOqfWF5jQFWS6LvNY5N1c1m0aFS/9qQX+sp727LIVVvOT+S/f7RQ==", null, false, "88ea6218-578b-4211-8a78-594d0081dc82", false, "admin" });

            migrationBuilder.InsertData(
                table: "Suppliers",
                columns: new[] { "SupplierCode", "Address", "City", "Company", "ContactPerson", "Country", "Email", "PhoneNumber", "PostalCode", "Region", "SupplierName" },
                values: new object[] { "SUP001", "123 Main St", "New York", "Global Tech Inc.", "John Doe", "USA", "john.doe@globaltech.com", "+15551234567", "12345", "NY", "Global Electronics" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1");

            migrationBuilder.DeleteData(
                table: "Suppliers",
                keyColumn: "SupplierCode",
                keyValue: "SUP001");
        }
    }
}
