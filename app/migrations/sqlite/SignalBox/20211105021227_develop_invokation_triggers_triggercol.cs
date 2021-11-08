using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_invokation_triggers_triggercol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TriggerCollection",
                table: "Recommenders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trigger",
                table: "ProductRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trigger",
                table: "ParameterSetRecommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Trigger",
                table: "ItemsRecommendations",
                type: "TEXT",
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
