using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_filterselectaggregate_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "LastCompleted",
                table: "FeatureGenerators",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastEnqueued",
                table: "FeatureGenerators",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCompleted",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "LastEnqueued",
                table: "FeatureGenerators");
        }
    }
}
