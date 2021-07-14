using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_tracked_user_features : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Scope",
                table: "ApiKeys",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CommonId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackedUserFeatures",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackedUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    FeatureId = table.Column<long>(type: "INTEGER", nullable: false),
                    NumericValue = table.Column<double>(type: "REAL", nullable: true),
                    StringValue = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUserFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedUserFeatures_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackedUserFeatures_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Features_CommonId",
                table: "Features",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserFeatures_FeatureId_TrackedUserId_Version",
                table: "TrackedUserFeatures",
                columns: new[] { "FeatureId", "TrackedUserId", "Version" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserFeatures_TrackedUserId",
                table: "TrackedUserFeatures",
                column: "TrackedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackedUserFeatures");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropColumn(
                name: "Scope",
                table: "ApiKeys");
        }
    }
}
