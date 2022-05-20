using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_argument_promo_rule_5726 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArgumentRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CampaignId = table.Column<long>(type: "bigint", nullable: false),
                    ArgumentId = table.Column<long>(type: "bigint", nullable: false),
                    ArgumentValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromotionId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArgumentRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArgumentRules_CampaignArgument_ArgumentId",
                        column: x => x.ArgumentId,
                        principalTable: "CampaignArgument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArgumentRules_RecommendableItems_PromotionId",
                        column: x => x.PromotionId,
                        principalTable: "RecommendableItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArgumentRules_Recommenders_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Recommenders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentRules_ArgumentId",
                table: "ArgumentRules",
                column: "ArgumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentRules_CampaignId",
                table: "ArgumentRules",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentRules_PromotionId",
                table: "ArgumentRules",
                column: "PromotionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArgumentRules");
        }
    }
}
