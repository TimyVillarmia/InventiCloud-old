using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class DefaultRoleandAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderedDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "SalesOrders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DestinationBranchId",
                table: "SalesOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "SalesOrders",
                type: "decimal(19,2)",
                precision: 19,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "your-admin-user-id-guid", 0, null, "d93b69be-ec2a-4abf-9527-a1da647a02a4", "admin@example.com", true, true, null, "ADMIN@EXAMPLE.COM", "ADMIN@EXAMPLE.COM", "AQAAAAIAAYagAAAAEN46Bsay+6kqV8Kb1Gr1b57ldpxI0cu9ADlgDXwSZ5YRJcHXOQH7oo+29hbPAm5vqA==", null, false, "3e71febc-92b7-4e43-8b3d-2291b1a71f19", false, "admin@example.com" });

            migrationBuilder.CreateIndex(
                name: "IX_SalesOrders_CreatedById",
                table: "SalesOrders",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_AspNetUsers_CreatedById",
                table: "SalesOrders",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_AspNetUsers_CreatedById",
                table: "SalesOrders");

            migrationBuilder.DropIndex(
                name: "IX_SalesOrders_CreatedById",
                table: "SalesOrders");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-admin-user-id-guid");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "DestinationBranchId",
                table: "SalesOrders");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "SalesOrders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderedDate",
                table: "SalesOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
