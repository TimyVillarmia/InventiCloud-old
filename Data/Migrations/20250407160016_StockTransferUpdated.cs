using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class StockTransferUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedDate",
                table: "StockTransfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PreviousQuantity",
                table: "StockTransferItems",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-admin-user-id-guid",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8226caf9-4371-4084-8514-abfcbb78f5fb", "AQAAAAIAAYagAAAAEDJ7Yb2gcvzWuONi4ZFEF0GDY0fpDnB4QSGhUmJRNfJ2b9wHWD5iKKXd8wp7AwkutA==", "863f2ecb-66dd-4e97-bbd3-a225ca8a272a" });

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 1,
                column: "StatusName",
                value: "Requested");

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 2,
                column: "StatusName",
                value: "Approved");

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 3,
                column: "StatusName",
                value: "Completed");

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 4,
                column: "StatusName",
                value: "Rejected");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectedDate",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "PreviousQuantity",
                table: "StockTransferItems");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-admin-user-id-guid",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "78ed4d1b-93cf-40e0-8e94-aaf5357598db", "AQAAAAIAAYagAAAAEKc2raTJJ7rjfkujhgrx8AGYeZqIYIdYL6q/zIHU08eNRQlMxz+cqt/OB3nGoS3BRg==", "2da19517-6475-427b-8f15-0df3938284e8" });

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 1,
                column: "StatusName",
                value: "Allocated");

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 2,
                column: "StatusName",
                value: "In Transit");

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 3,
                column: "StatusName",
                value: "Cancelled");

            migrationBuilder.UpdateData(
                table: "StockTransferStatuses",
                keyColumn: "StockTransferStatusId",
                keyValue: 4,
                column: "StatusName",
                value: "Completed");
        }
    }
}
