using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_segment_environment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Environments_EnvironmentId",
                table: "Segments");

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Environments_EnvironmentId",
                table: "Segments",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Environments_EnvironmentId",
                table: "Segments");

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Environments_EnvironmentId",
                table: "Segments",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
