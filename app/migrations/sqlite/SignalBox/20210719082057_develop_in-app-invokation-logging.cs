using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_inappinvokationlogging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecommenderType",
                table: "Recommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommenderType",
                table: "ProductRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommenderType",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvokationLogEntry",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecommenderType = table.Column<string>(type: "TEXT", nullable: false),
                    RecommenderId = table.Column<long>(type: "INTEGER", nullable: false),
                    Success = table.Column<bool>(type: "INTEGER", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    InvokeStarted = table.Column<long>(type: "INTEGER", nullable: false),
                    InvokeEnded = table.Column<long>(type: "INTEGER", nullable: true),
                    CorrelatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    TrackedUserId = table.Column<long>(type: "INTEGER", nullable: true),
                    ParameterSetRecommenderId = table.Column<long>(type: "INTEGER", nullable: true),
                    ProductRecommenderId = table.Column<long>(type: "INTEGER", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvokationLogEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvokationLogEntry_ParameterSetRecommenders_ParameterSetRecommenderId",
                        column: x => x.ParameterSetRecommenderId,
                        principalTable: "ParameterSetRecommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvokationLogEntry_ProductRecommenders_ProductRecommenderId",
                        column: x => x.ProductRecommenderId,
                        principalTable: "ProductRecommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvokationLogEntry_RecommendationCorrelators_CorrelatorId",
                        column: x => x.CorrelatorId,
                        principalTable: "RecommendationCorrelators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvokationLogEntry_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_Timestamp",
                table: "TrackedUserEvents",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_CorrelatorId",
                table: "InvokationLogEntry",
                column: "CorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_InvokeStarted",
                table: "InvokationLogEntry",
                column: "InvokeStarted");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_ParameterSetRecommenderId",
                table: "InvokationLogEntry",
                column: "ParameterSetRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_ProductRecommenderId",
                table: "InvokationLogEntry",
                column: "ProductRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_TrackedUserId",
                table: "InvokationLogEntry",
                column: "TrackedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvokationLogEntry");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_Timestamp",
                table: "TrackedUserEvents");

            migrationBuilder.DropColumn(
                name: "RecommenderType",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "RecommenderType",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "RecommenderType",
                table: "ParameterSetRecommendations");
        }
    }
}
