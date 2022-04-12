using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_trackusereventindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_TrackedUserId",
                table: "TrackedUserEvents");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_TrackedUserId_EnvironmentId_Timestamp",
                table: "TrackedUserEvents",
                columns: new[] { "TrackedUserId", "EnvironmentId", "Timestamp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_TrackedUserId_EnvironmentId_Timestamp",
                table: "TrackedUserEvents");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_TrackedUserId",
                table: "TrackedUserEvents",
                column: "TrackedUserId");
        }
    }
}
