using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_deferred_delivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeferredDeliveries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastAttemptedDelivery = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    RecommendationId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeferredDeliveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeferredDeliveries_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeferredDeliveries_ItemsRecommendations_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "ItemsRecommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeferredDeliveries_ChannelId_RecommendationId",
                table: "DeferredDeliveries",
                columns: new[] { "ChannelId", "RecommendationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeferredDeliveries_RecommendationId",
                table: "DeferredDeliveries",
                column: "RecommendationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeferredDeliveries");
        }
    }
}
