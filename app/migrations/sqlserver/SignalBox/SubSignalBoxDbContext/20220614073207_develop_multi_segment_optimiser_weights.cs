using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_multi_segment_optimiser_weights : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PromotionOptimiserWeight_OptimiserId_SegmentId_PromotionId",
                table: "PromotionOptimiserWeight",
                columns: new[] { "OptimiserId", "SegmentId", "PromotionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PromotionOptimiserWeight_OptimiserId_SegmentId_PromotionId",
                table: "PromotionOptimiserWeight");
        }
    }
}
