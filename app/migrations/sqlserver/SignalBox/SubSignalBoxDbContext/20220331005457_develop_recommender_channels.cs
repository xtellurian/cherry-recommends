using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_recommender_channels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecommenderChannel",
                columns: table => new
                {
                    ChannelsId = table.Column<long>(type: "bigint", nullable: false),
                    RecommendersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommenderChannel", x => new { x.ChannelsId, x.RecommendersId });
                    table.ForeignKey(
                        name: "FK_RecommenderChannel_Channels_ChannelsId",
                        column: x => x.ChannelsId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommenderChannel_Recommenders_RecommendersId",
                        column: x => x.RecommendersId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommenderChannel_RecommendersId",
                table: "RecommenderChannel",
                column: "RecommendersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommenderChannel");
        }
    }
}
