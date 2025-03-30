using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class StockAdjustmentReasonStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Customers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a4d331f6-9bef-4449-803e-7a48d5e0b512", "AQAAAAIAAYagAAAAEFM2RhOfhrQO0BFAHBaCMEuavU8P2Ch91i4LmpanZjF5kr3FUwmRiPLbkyY8wa/rsw==", "be7890e5-6b6c-4151-b149-252503ccd73b" });

            migrationBuilder.InsertData(
                table: "StockAdjustmentReasons",
                columns: new[] { "StockAdjustmentReasonId", "Reason" },
                values: new object[,]
                {
                    { 1, "Damaged/Defective" },
                    { 2, "Loss/Shrinkage" },
                    { 3, "Unexpected Receipt/Found" },
                    { 4, "Physical Count Variance" },
                    { 5, "Expired/Obsolete" },
                    { 6, "Initial Inventory Adjustment:" }
                });

            migrationBuilder.InsertData(
                table: "StockAdjustmentStatuses",
                columns: new[] { "StockAdjustmentStatusId", "StatusName" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Completed" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "StockAdjustmentReasonId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "StockAdjustmentReasonId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "StockAdjustmentReasonId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "StockAdjustmentReasonId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "StockAdjustmentReasonId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentReasons",
                keyColumn: "StockAdjustmentReasonId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentStatuses",
                keyColumn: "StockAdjustmentStatusId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StockAdjustmentStatuses",
                keyColumn: "StockAdjustmentStatusId",
                keyValue: 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Customers",
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
                values: new object[] { "b27b2e2d-362d-4413-9de8-70c09f0d0f5c", "AQAAAAIAAYagAAAAEHJ1dFH8uVixHpatLZoODmyloMqGbduzvWyBE6dyKJZSBi4faDS/QSyJqCZi4q8Z3w==", "2b55f7f2-2e30-464a-93af-244bd5b3899f" });
        }
    }
}
