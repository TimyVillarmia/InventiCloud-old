using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class StockTransferUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Branches_FromBranchId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Branches_ToBranchId",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "StockTransfers");

            migrationBuilder.RenameColumn(
                name: "TransferDate",
                table: "StockTransfers",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ToBranchId",
                table: "StockTransfers",
                newName: "SourceBranchId");

            migrationBuilder.RenameColumn(
                name: "FromBranchId",
                table: "StockTransfers",
                newName: "DestinationBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransfers_ToBranchId",
                table: "StockTransfers",
                newName: "IX_StockTransfers_SourceBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransfers_FromBranchId",
                table: "StockTransfers",
                newName: "IX_StockTransfers_DestinationBranchId");

            migrationBuilder.RenameColumn(
                name: "StockTransferDetailId",
                table: "StockTransferDetails",
                newName: "StockTransferItemlId");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "StockTransfers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCompleted",
                table: "StockTransfers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "StockTransfers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a369dcae-6b8e-4a44-84c2-d3d72c9cf7c6", "AQAAAAIAAYagAAAAEOlYwkg7mT/dqkAjLOOvIcO2PcJ4WVL7LLi6b5YhY4J0NBNMi6fHnA6frVJkF9E7HQ==", "c737ea21-7bba-485c-8011-8ee035d06776" });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_CreatedById",
                table: "StockTransfers",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_AspNetUsers_CreatedById",
                table: "StockTransfers",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Branches_DestinationBranchId",
                table: "StockTransfers",
                column: "DestinationBranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Branches_SourceBranchId",
                table: "StockTransfers",
                column: "SourceBranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_AspNetUsers_CreatedById",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Branches_DestinationBranchId",
                table: "StockTransfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransfers_Branches_SourceBranchId",
                table: "StockTransfers");

            migrationBuilder.DropIndex(
                name: "IX_StockTransfers_CreatedById",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "DateCompleted",
                table: "StockTransfers");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "StockTransfers");

            migrationBuilder.RenameColumn(
                name: "SourceBranchId",
                table: "StockTransfers",
                newName: "ToBranchId");

            migrationBuilder.RenameColumn(
                name: "DestinationBranchId",
                table: "StockTransfers",
                newName: "FromBranchId");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "StockTransfers",
                newName: "TransferDate");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransfers_SourceBranchId",
                table: "StockTransfers",
                newName: "IX_StockTransfers_ToBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransfers_DestinationBranchId",
                table: "StockTransfers",
                newName: "IX_StockTransfers_FromBranchId");

            migrationBuilder.RenameColumn(
                name: "StockTransferItemlId",
                table: "StockTransferDetails",
                newName: "StockTransferDetailId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedDate",
                table: "StockTransfers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cb7366b5-b661-4ca3-9bc5-5f6362a03a8a", "AQAAAAIAAYagAAAAEDUJOxOOg4fFWs2uZ1n11QVMZQju1AcwuQJwgqweZAd2Sq/jciiu/bpSYGkh7GU1/w==", "5b62330c-f87b-4c15-a55c-afef64bdef05" });

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Branches_FromBranchId",
                table: "StockTransfers",
                column: "FromBranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransfers_Branches_ToBranchId",
                table: "StockTransfers",
                column: "ToBranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
