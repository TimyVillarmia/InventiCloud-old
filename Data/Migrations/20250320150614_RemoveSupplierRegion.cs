using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSupplierRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Suppliers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cb7366b5-b661-4ca3-9bc5-5f6362a03a8a", "AQAAAAIAAYagAAAAEDUJOxOOg4fFWs2uZ1n11QVMZQju1AcwuQJwgqweZAd2Sq/jciiu/bpSYGkh7GU1/w==", "5b62330c-f87b-4c15-a55c-afef64bdef05" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad04d457-eff5-4d96-afb6-0f9063b9ea27", "AQAAAAIAAYagAAAAEKa0U/uFFE9xPmTkvdv41jKUTt2jE06LmOjrIYw0vjVgB1G7sXAHPzFztF8ehmPKOA==", "652c3c0b-773b-4051-b2d9-11adbfab7114" });

            migrationBuilder.UpdateData(
                table: "Suppliers",
                keyColumn: "SupplierCode",
                keyValue: "SUP001",
                column: "Region",
                value: "NY");
        }
    }
}
