using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_trackrecommendationoutcomes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecommendationCorrelatorId",
                table: "TrackedUserEvents",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecommendationCorrelatorId",
                table: "Recommendations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "ParameterSetRecommendations",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "Created",
                table: "ParameterSetRecommendations",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                name: "RecommendationCorrelatorId",
                table: "ParameterSetRecommendations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecommendationCorrelators",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationCorrelators", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_RecommendationCorrelatorId",
                table: "Recommendations",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommendations_RecommendationCorrelatorId",
                table: "ParameterSetRecommendations",
                column: "RecommendationCorrelatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "ParameterSetRecommendations",
                column: "RecommendationCorrelatorId",
                principalTable: "RecommendationCorrelators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "Recommendations",
                column: "RecommendationCorrelatorId",
                principalTable: "RecommendationCorrelators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                table: "Recommendations");

            migrationBuilder.DropTable(
                name: "RecommendationCorrelators");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_RecommendationCorrelatorId",
                table: "Recommendations");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommendations_RecommendationCorrelatorId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "RecommendationCorrelatorId",
                table: "TrackedUserEvents");

            migrationBuilder.DropColumn(
                name: "RecommendationCorrelatorId",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "RecommendationCorrelatorId",
                table: "ParameterSetRecommendations");

            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "ParameterSetRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<long>(
                name: "Created",
                table: "ParameterSetRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
