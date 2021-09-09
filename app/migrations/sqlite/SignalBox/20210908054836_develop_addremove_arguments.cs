using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_addremove_arguments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_Touchpoints_TouchpointId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_TouchpointId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "TouchpointId",
                table: "Recommenders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TouchpointId",
                table: "Recommenders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_TouchpointId",
                table: "Recommenders",
                column: "TouchpointId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_Touchpoints_TouchpointId",
                table: "Recommenders",
                column: "TouchpointId",
                principalTable: "Touchpoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
