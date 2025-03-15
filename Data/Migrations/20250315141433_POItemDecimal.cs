using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class POItemDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "PurchaseOrderItems",
                type: "decimal(19,2)",
                precision: 19,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldPrecision: 19,
                oldScale: 4);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "PurchaseOrderItems",
                type: "decimal(19,2)",
                precision: 19,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,4)",
                oldPrecision: 19,
                oldScale: 4);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ad04d457-eff5-4d96-afb6-0f9063b9ea27", "AQAAAAIAAYagAAAAEKa0U/uFFE9xPmTkvdv41jKUTt2jE06LmOjrIYw0vjVgB1G7sXAHPzFztF8ehmPKOA==", "652c3c0b-773b-4051-b2d9-11adbfab7114" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "PurchaseOrderItems",
                type: "decimal(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,2)",
                oldPrecision: 19,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "SubTotal",
                table: "PurchaseOrderItems",
                type: "decimal(19,4)",
                precision: 19,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(19,2)",
                oldPrecision: 19,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9c2eed7-0676-420a-b1ca-4181d8c4e1e3", "AQAAAAIAAYagAAAAEOWcvQiZC/mJzfY45MkbIRH0HLYdMC+ZhywpCvwSiba5PhIdCq25Gc6okYWwvXUDgg==", "dc255fb5-61fe-4727-bc4d-fb681dbd89ee" });
        }
    }
}
