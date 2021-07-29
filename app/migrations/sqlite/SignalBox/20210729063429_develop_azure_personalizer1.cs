using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_azure_personalizer1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "RecommendationCorrelators",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "Created",
                table: "RecommendationCorrelators",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<long>(
                name: "ModelRegistrationId",
                table: "RecommendationCorrelators",
                type: "INTEGER",
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

            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "RecommendationCorrelators",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<long>(
                name: "Created",
                table: "RecommendationCorrelators",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
