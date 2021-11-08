using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_invokation_triggers_triggercol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TriggerCollection",
                table: "Recommenders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trigger",
                table: "ProductRecommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trigger",
                table: "ParameterSetRecommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trigger",
                table: "ItemsRecommendations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerCollection",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "Trigger",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "Trigger",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "Trigger",
                table: "ItemsRecommendations");
        }
    }
}
