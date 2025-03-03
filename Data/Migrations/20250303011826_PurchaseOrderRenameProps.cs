using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class PurchaseOrderRenameProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchAccount_AspNetUsers_ApplicationUserId",
                table: "BranchAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_BranchAccount_Branches_BranchId",
                table: "BranchAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedBy",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Branches_DestinationBranch",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchAccount",
                table: "BranchAccount");

            migrationBuilder.RenameTable(
                name: "BranchAccount",
                newName: "BranchAccounts");

            migrationBuilder.RenameColumn(
                name: "DestinationBranch",
                table: "PurchaseOrders",
                newName: "DestinationBranchId");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "PurchaseOrders",
                newName: "CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_DestinationBranch",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_DestinationBranchId");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_CreatedBy",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_BranchAccount_BranchId",
                table: "BranchAccounts",
                newName: "IX_BranchAccounts_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_BranchAccount_ApplicationUserId",
                table: "BranchAccounts",
                newName: "IX_BranchAccounts_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchAccounts",
                table: "BranchAccounts",
                column: "BranchAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchAccounts_AspNetUsers_ApplicationUserId",
                table: "BranchAccounts",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BranchAccounts_Branches_BranchId",
                table: "BranchAccounts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                table: "PurchaseOrders",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Branches_DestinationBranchId",
                table: "PurchaseOrders",
                column: "DestinationBranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BranchAccounts_AspNetUsers_ApplicationUserId",
                table: "BranchAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_BranchAccounts_Branches_BranchId",
                table: "BranchAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedById",
                table: "PurchaseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_Branches_DestinationBranchId",
                table: "PurchaseOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BranchAccounts",
                table: "BranchAccounts");

            migrationBuilder.RenameTable(
                name: "BranchAccounts",
                newName: "BranchAccount");

            migrationBuilder.RenameColumn(
                name: "DestinationBranchId",
                table: "PurchaseOrders",
                newName: "DestinationBranch");

            migrationBuilder.RenameColumn(
                name: "CreatedById",
                table: "PurchaseOrders",
                newName: "CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_DestinationBranchId",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_DestinationBranch");

            migrationBuilder.RenameIndex(
                name: "IX_PurchaseOrders_CreatedById",
                table: "PurchaseOrders",
                newName: "IX_PurchaseOrders_CreatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_BranchAccounts_BranchId",
                table: "BranchAccount",
                newName: "IX_BranchAccount_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_BranchAccounts_ApplicationUserId",
                table: "BranchAccount",
                newName: "IX_BranchAccount_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BranchAccount",
                table: "BranchAccount",
                column: "BranchAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_BranchAccount_AspNetUsers_ApplicationUserId",
                table: "BranchAccount",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BranchAccount_Branches_BranchId",
                table: "BranchAccount",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_AspNetUsers_CreatedBy",
                table: "PurchaseOrders",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_Branches_DestinationBranch",
                table: "PurchaseOrders",
                column: "DestinationBranch",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
