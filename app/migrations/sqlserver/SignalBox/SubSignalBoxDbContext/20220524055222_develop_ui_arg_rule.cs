using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_ui_arg_rule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArgumentValue",
                table: "ArgumentRules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentRules_ArgumentId_PromotionId_CampaignId_ArgumentValue",
                table: "ArgumentRules",
                columns: new[] { "ArgumentId", "PromotionId", "CampaignId", "ArgumentValue" },
                unique: true,
                filter: "[PromotionId] IS NOT NULL AND [ArgumentValue] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ArgumentRules_ArgumentId_PromotionId_CampaignId_ArgumentValue",
                table: "ArgumentRules");

            migrationBuilder.AlterColumn<string>(
                name: "ArgumentValue",
                table: "ArgumentRules",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
