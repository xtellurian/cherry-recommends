using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_tracked_user_actions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrackedUserActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommonUserId = table.Column<string>(type: "TEXT", nullable: false),
                    EventId = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false),
                    RecommendationCorrelatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    IntegratedSystemId = table.Column<long>(type: "INTEGER", nullable: true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    ActionName = table.Column<string>(type: "TEXT", nullable: true),
                    ActionValue = table.Column<string>(type: "TEXT", nullable: true),
                    ValueType = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUserActions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_ActionName",
                table: "TrackedUserActions",
                column: "ActionName");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_CommonUserId",
                table: "TrackedUserActions",
                column: "CommonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_Timestamp",
                table: "TrackedUserActions",
                column: "Timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackedUserActions");
        }
    }
}
