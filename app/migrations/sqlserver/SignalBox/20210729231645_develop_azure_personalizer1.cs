using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_azure_personalizer1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "RecommendationCorrelators",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "RecommendationCorrelators",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<long>(
                name: "ModelRegistrationId",
                table: "RecommendationCorrelators",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationCorrelators_ModelRegistrationId",
                table: "RecommendationCorrelators",
                column: "ModelRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendationCorrelators_ModelRegistrations_ModelRegistrationId",
                table: "RecommendationCorrelators",
                column: "ModelRegistrationId",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecommendationCorrelators_ModelRegistrations_ModelRegistrationId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropIndex(
                name: "IX_RecommendationCorrelators_ModelRegistrationId",
                table: "RecommendationCorrelators");

            migrationBuilder.DropColumn(
                name: "ModelRegistrationId",
                table: "RecommendationCorrelators");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "RecommendationCorrelators",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Created",
                table: "RecommendationCorrelators",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
