using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_trackrecommendationoutcomes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecommendationCorrelatorId",
                table: "TrackedUserEvents",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecommendationCorrelatorId",
                table: "Recommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "ParameterSetRecommendations",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "ParameterSetRecommendations",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<long>(
                name: "RecommendationCorrelatorId",
                table: "ParameterSetRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecommendationCorrelators",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
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

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "ParameterSetRecommendations",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "ParameterSetRecommendations",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
