using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_selectrecommenderfeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureRecommenderEntityBase",
                columns: table => new
                {
                    LearningFeaturesId = table.Column<long>(type: "INTEGER", nullable: false),
                    RecommendersId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureRecommenderEntityBase", x => new { x.LearningFeaturesId, x.RecommendersId });
                    table.ForeignKey(
                        name: "FK_FeatureRecommenderEntityBase_Features_LearningFeaturesId",
                        column: x => x.LearningFeaturesId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureRecommenderEntityBase_Recommenders_RecommendersId",
                        column: x => x.RecommendersId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureRecommenderEntityBase_RecommendersId",
                table: "FeatureRecommenderEntityBase",
                column: "RecommendersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureRecommenderEntityBase");
        }
    }
}
