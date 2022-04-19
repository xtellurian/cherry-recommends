using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class updatewebchannelproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "Host",
                table: "Channels",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PopupDelay",
                table: "Channels",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecommenderIdToInvoke",
                table: "Channels",
                type: "bigint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Host",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "PopupDelay",
                table: "Channels");

            migrationBuilder.DropColumn(
                name: "RecommenderIdToInvoke",
                table: "Channels");
        }
    }
}
