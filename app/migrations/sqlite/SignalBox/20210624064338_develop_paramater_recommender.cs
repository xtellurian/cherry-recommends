using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_paramater_recommender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModelRegistrationId",
                table: "ParameterSetRecommenders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelInput",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelInputType",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelOutput",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelOutputType",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommenders_ModelRegistrationId",
                table: "ParameterSetRecommenders",
                column: "ModelRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommenders_ModelRegistrations_ModelRegistrationId",
                table: "ParameterSetRecommenders",
                column: "ModelRegistrationId",
                principalTable: "ModelRegistrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommenders_ModelRegistrations_ModelRegistrationId",
                table: "ParameterSetRecommenders");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommenders_ModelRegistrationId",
                table: "ParameterSetRecommenders");

            migrationBuilder.DropColumn(
                name: "ModelRegistrationId",
                table: "ParameterSetRecommenders");

            migrationBuilder.DropColumn(
                name: "ModelInput",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "ModelInputType",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "ModelOutput",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "ModelOutputType",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ParameterSetRecommendations");
        }
    }
}
