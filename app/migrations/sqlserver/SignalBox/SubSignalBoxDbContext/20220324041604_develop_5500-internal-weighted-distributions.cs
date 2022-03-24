using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_5500internalweighteddistributions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseOptimiser",
                table: "Recommenders",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PromotionOptimisers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecommenderId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EnvironmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionOptimisers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionOptimisers_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PromotionOptimisers_Recommenders_RecommenderId",
                        column: x => x.RecommenderId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionOptimiserWeight",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OptimiserId = table.Column<long>(type: "bigint", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    SegmentId = table.Column<long>(type: "bigint", nullable: true),
                    PromotionId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionOptimiserWeight", x => new { x.OptimiserId, x.Id });
                    table.ForeignKey(
                        name: "FK_PromotionOptimiserWeight_PromotionOptimisers_OptimiserId",
                        column: x => x.OptimiserId,
                        principalTable: "PromotionOptimisers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionOptimiserWeight_RecommendableItems_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionOptimiserWeight_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOptimisers_EnvironmentId",
                table: "PromotionOptimisers",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOptimisers_RecommenderId",
                table: "PromotionOptimisers",
                column: "RecommenderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOptimiserWeight_PromotionId",
                table: "PromotionOptimiserWeight",
                column: "PromotionId");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionOptimiserWeight_SegmentId",
                table: "PromotionOptimiserWeight",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromotionOptimiserWeight");

            migrationBuilder.DropTable(
                name: "PromotionOptimisers");

            migrationBuilder.DropColumn(
                name: "UseOptimiser",
                table: "Recommenders");
        }
    }
}
