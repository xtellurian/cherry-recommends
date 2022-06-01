using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_offer_grossrevenue_datatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "GrossRevenue",
                table: "Offers",
                type: "float",
                nullable: true,
                defaultValue: 1.0,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true,
                oldDefaultValue: 1f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "GrossRevenue",
                table: "Offers",
                type: "real",
                nullable: true,
                defaultValue: 1f,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValue: 1.0);
        }
    }
}
