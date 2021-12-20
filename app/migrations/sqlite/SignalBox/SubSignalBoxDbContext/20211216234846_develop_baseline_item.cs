using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_baseline_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultItemId",
                table: "Recommenders");

            migrationBuilder.RenameColumn(
                name: "DefaultItemId",
                table: "Recommenders",
                newName: "BaselineItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommenders_DefaultItemId",
                table: "Recommenders",
                newName: "IX_Recommenders_BaselineItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_RecommendableItems_BaselineItemId",
                table: "Recommenders",
                column: "BaselineItemId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_RecommendableItems_BaselineItemId",
                table: "Recommenders");

            migrationBuilder.RenameColumn(
                name: "BaselineItemId",
                table: "Recommenders",
                newName: "DefaultItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Recommenders_BaselineItemId",
                table: "Recommenders",
                newName: "IX_Recommenders_DefaultItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_RecommendableItems_DefaultItemId",
                table: "Recommenders",
                column: "DefaultItemId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
