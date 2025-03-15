using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventiCloud.Migrations
{
    /// <inheritdoc />
    public partial class POItemsPRoductMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a9c2eed7-0676-420a-b1ca-4181d8c4e1e3", "AQAAAAIAAYagAAAAEOWcvQiZC/mJzfY45MkbIRH0HLYdMC+ZhywpCvwSiba5PhIdCq25Gc6okYWwvXUDgg==", "dc255fb5-61fe-4727-bc4d-fb681dbd89ee" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "your-user-id-1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ce9e0c94-5885-4079-9b77-fdd5b62f5c94", "AQAAAAIAAYagAAAAEEX/fBHChLdzWrnI1qUjFTOpO2J15fVm/iSRgoy/CdpgrT7A4SFszQWmikTGHQ8LQQ==", "a3d5bd5c-16da-45f1-8b08-8f1bdca0d4e2" });
        }
    }
}
