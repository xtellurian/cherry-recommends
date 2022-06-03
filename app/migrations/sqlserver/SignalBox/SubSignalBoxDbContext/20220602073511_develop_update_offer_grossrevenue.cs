using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_update_offer_grossrevenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "GrossRevenue",
                table: "Offers",
                type: "float",
                nullable: true,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValue: 1.0);

            // GrossRevenue is only for redeemed offers
            migrationBuilder.Sql(@"
                UPDATE  Offers
                SET     GrossRevenue = 0
                WHERE   [State] <> 'Redeemed'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "GrossRevenue",
                table: "Offers",
                type: "float",
                nullable: true,
                defaultValue: 1.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true,
                oldDefaultValue: 0.0);
        }
    }
}
