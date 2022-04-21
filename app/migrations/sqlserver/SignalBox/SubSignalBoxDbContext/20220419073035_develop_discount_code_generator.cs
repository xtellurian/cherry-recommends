using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_discount_code_generator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndsAt",
                table: "DiscountCodes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartsAt",
                table: "DiscountCodes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountCodeItemsRecommendation",
                columns: table => new
                {
                    DiscountCodesId = table.Column<long>(type: "bigint", nullable: false),
                    RecommendationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeItemsRecommendation", x => new { x.DiscountCodesId, x.RecommendationsId });
                    table.ForeignKey(
                        name: "FK_DiscountCodeItemsRecommendation_DiscountCodes_DiscountCodesId",
                        column: x => x.DiscountCodesId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountCodeItemsRecommendation_ItemsRecommendations_RecommendationsId",
                        column: x => x.RecommendationsId,
                        principalTable: "ItemsRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeItemsRecommendation_RecommendationsId",
                table: "DiscountCodeItemsRecommendation",
                column: "RecommendationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountCodeItemsRecommendation");

            migrationBuilder.DropColumn(
                name: "EndsAt",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "StartsAt",
                table: "DiscountCodes");
        }
    }
}
