using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_product_recommenders2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecommenderId",
                table: "ProductRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecommenderId",
                table: "ParameterSetRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommendations_RecommenderId",
                table: "ParameterSetRecommendations",
                column: "RecommenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations",
                column: "RecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ProductRecommendations_RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommendations_RecommenderId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropColumn(
                name: "RecommenderId",
                table: "ParameterSetRecommendations");
        }
    }
}
