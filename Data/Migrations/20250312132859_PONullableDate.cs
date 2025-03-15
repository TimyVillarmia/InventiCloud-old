using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class PONullableDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ce9e0c94-5885-4079-9b77-fdd5b62f5c94", "AQAAAAIAAYagAAAAEEX/fBHChLdzWrnI1qUjFTOpO2J15fVm/iSRgoy/CdpgrT7A4SFszQWmikTGHQ8LQQ==", "a3d5bd5c-16da-45f1-8b08-8f1bdca0d4e2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PurchasedDate",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c247c5e7-0e47-4da7-a607-205fcb25110e", "AQAAAAIAAYagAAAAEA3LpOvTJs9/lcCOqfWF5jQFWS6LvNY5N1c1m0aFS/9qQX+sp727LIVVvOT+S/f7RQ==", "88ea6218-578b-4211-8a78-594d0081dc82" });
        }
    }
}
