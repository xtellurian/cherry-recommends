using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_filterselectaggregate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilterSelectAggregateSteps",
                table: "FeatureGenerators",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilterSelectAggregateSteps",
                table: "FeatureGenerators");
        }
    }
}
