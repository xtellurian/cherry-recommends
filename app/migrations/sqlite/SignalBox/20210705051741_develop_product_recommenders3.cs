using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_product_recommenders3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TrackedUserId",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TrackedUserId",
                table: "ParameterSetRecommendations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_ProductId",
                table: "ProductRecommendations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_TrackedUserId",
                table: "ProductRecommendations",
                column: "TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommendations_TrackedUserId",
                table: "ParameterSetRecommendations",
                column: "TrackedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_TrackedUsers_TrackedUserId",
                table: "ParameterSetRecommendations",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_Products_ProductId",
                table: "ProductRecommendations",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_TrackedUsers_TrackedUserId",
                table: "ProductRecommendations",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_TrackedUsers_TrackedUserId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_Products_ProductId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_TrackedUsers_TrackedUserId",
                table: "ProductRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ProductRecommendations_ProductId",
                table: "ProductRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ProductRecommendations_TrackedUserId",
                table: "ProductRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommendations_TrackedUserId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "TrackedUserId",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "TrackedUserId",
                table: "ParameterSetRecommendations");
        }
    }
}
