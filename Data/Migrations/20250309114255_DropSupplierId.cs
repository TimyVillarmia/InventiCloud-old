using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class DropSupplierId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierID",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_SupplierID",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "SupplierID",
                table: "PurchaseOrders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedArrival",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "SupplierCode",
                table: "PurchaseOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "SupplierCode");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierCode",
                table: "PurchaseOrders",
                column: "SupplierCode");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierCode",
                table: "PurchaseOrders",
                column: "SupplierCode",
                principalTable: "Suppliers",
                principalColumn: "SupplierCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierCode",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_SupplierCode",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "SupplierCode",
                table: "PurchaseOrders");

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EstimatedArrival",
                table: "PurchaseOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierID",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_SupplierID",
                table: "PurchaseOrders",
                column: "SupplierID");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Suppliers_SupplierID",
                table: "PurchaseOrders",
                column: "SupplierID",
                principalTable: "Suppliers",
                principalColumn: "SupplierId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
