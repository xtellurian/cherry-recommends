using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_discount_code_fix_ondelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_RecommendableItems_PromotionId",
                table: "DiscountCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_RecommendableItems_PromotionId",
                table: "DiscountCodes",
                column: "PromotionId",
                principalTable: "RecommendableItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_RecommendableItems_PromotionId",
                table: "DiscountCodes");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_RecommendableItems_PromotionId",
                table: "DiscountCodes",
                column: "PromotionId",
                principalTable: "RecommendableItems",
                principalColumn: "Id");
        }
    }
}
