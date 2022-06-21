using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_offer_redeemedcount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RedeemedCount",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            string query = @"UPDATE Offers SET RedeemedCount = 1 WHERE [State] = 'Redeemed'";
            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedeemedCount",
                table: "Offers");
        }
    }
}
