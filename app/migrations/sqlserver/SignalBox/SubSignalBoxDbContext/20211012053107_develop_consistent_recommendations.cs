using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_consistent_recommendations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "ItemsRecommendations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "ProductRecommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "ParameterSetRecommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "ItemsRecommendations",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
