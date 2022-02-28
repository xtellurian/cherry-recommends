using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class update_benefit_value_default : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BenefitValue",
                table: "RecommendableItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 1.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "BenefitValue",
                table: "RecommendableItems",
                type: "float",
                nullable: false,
                defaultValue: 1.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldDefaultValue: 0.0);
        }
    }
}
