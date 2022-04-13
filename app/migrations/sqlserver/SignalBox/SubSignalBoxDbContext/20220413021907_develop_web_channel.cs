using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_web_channel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Environments_EnvironmentId",
                table: "Channels");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Environments_EnvironmentId",
                table: "Channels",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Environments_EnvironmentId",
                table: "Channels");

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Environments_EnvironmentId",
                table: "Channels",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
