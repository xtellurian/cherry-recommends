using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_offers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecommendationId = table.Column<long>(type: "bigint", nullable: false),
                    RedeemedPromotionId = table.Column<long>(type: "bigint", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedeemedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    GrossRevenue = table.Column<float>(type: "real", nullable: true, defaultValue: 1f),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EnvironmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_ItemsRecommendations_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "ItemsRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Offers_RecommendableItems_RedeemedPromotionId",
                        column: x => x.RedeemedPromotionId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_EnvironmentId",
                table: "Offers",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_RecommendationId",
                table: "Offers",
                column: "RecommendationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_RedeemedPromotionId",
                table: "Offers",
                column: "RedeemedPromotionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers");
        }
    }
}
