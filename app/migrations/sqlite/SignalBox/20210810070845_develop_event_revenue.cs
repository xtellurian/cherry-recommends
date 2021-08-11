using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_event_revenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrackedUserId",
                table: "TrackedUserEvents",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AssociatedRevenue",
                table: "TrackedUserActions",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TrackedUserEventId",
                table: "TrackedUserActions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TrackedUserId",
                table: "TrackedUserActions",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RewardSelectors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Category = table.Column<string>(type: "TEXT", nullable: true),
                    ActionName = table.Column<string>(type: "TEXT", nullable: true),
                    SelectorType = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardSelectors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_TrackedUserId",
                table: "TrackedUserEvents",
                column: "TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_TrackedUserEventId",
                table: "TrackedUserActions",
                column: "TrackedUserEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_TrackedUserId",
                table: "TrackedUserActions",
                column: "TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardSelectors_ActionName_SelectorType",
                table: "RewardSelectors",
                columns: new[] { "ActionName", "SelectorType" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserActions_TrackedUserEvents_TrackedUserEventId",
                table: "TrackedUserActions",
                column: "TrackedUserEventId",
                principalTable: "TrackedUserEvents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserActions_TrackedUsers_TrackedUserId",
                table: "TrackedUserActions",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserEvents_TrackedUsers_TrackedUserId",
                table: "TrackedUserEvents",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserActions_TrackedUserEvents_TrackedUserEventId",
                table: "TrackedUserActions");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserActions_TrackedUsers_TrackedUserId",
                table: "TrackedUserActions");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserEvents_TrackedUsers_TrackedUserId",
                table: "TrackedUserEvents");

            migrationBuilder.DropTable(
                name: "RewardSelectors");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_TrackedUserId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserActions_TrackedUserEventId",
                table: "TrackedUserActions");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserActions_TrackedUserId",
                table: "TrackedUserActions");

            migrationBuilder.DropColumn(
                name: "TrackedUserId",
                table: "TrackedUserEvents");

            migrationBuilder.DropColumn(
                name: "AssociatedRevenue",
                table: "TrackedUserActions");

            migrationBuilder.DropColumn(
                name: "TrackedUserEventId",
                table: "TrackedUserActions");

            migrationBuilder.DropColumn(
                name: "TrackedUserId",
                table: "TrackedUserActions");
        }
    }
}
