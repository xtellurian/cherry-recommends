using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_prototype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Recommendations_OfferRecommendationId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_OfferRecommendationId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "OfferRecommendationId",
                table: "Offers");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "TrackUserSystemMaps",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "TrackedUsers",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "TrackedUserEvents",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "Segments",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "Rules",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "Recommendations",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "PresentationOutcomes",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "Offers",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "ModelRegistrations",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "IntegratedSystems",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "Experiments",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastUpdated",
                table: "ApiKeys",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateTable(
                name: "OfferOfferRecommendation",
                columns: table => new
                {
                    OffersId = table.Column<long>(type: "bigint", nullable: false),
                    RecommendationsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferOfferRecommendation", x => new { x.OffersId, x.RecommendationsId });
                    table.ForeignKey(
                        name: "FK_OfferOfferRecommendation_Offers_OffersId",
                        column: x => x.OffersId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferOfferRecommendation_Recommendations_RecommendationsId",
                        column: x => x.RecommendationsId,
                        principalTable: "Recommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skus",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    SkuId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skus_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfferOfferRecommendation_RecommendationsId",
                table: "OfferOfferRecommendation",
                column: "RecommendationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Skus_ProductId",
                table: "Skus",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferOfferRecommendation");

            migrationBuilder.DropTable(
                name: "Skus");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "TrackedUsers");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "TrackedUserEvents");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "PresentationOutcomes");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "ModelRegistrations");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Experiments");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "ApiKeys");

            migrationBuilder.AddColumn<long>(
                name: "OfferRecommendationId",
                table: "Offers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_OfferRecommendationId",
                table: "Offers",
                column: "OfferRecommendationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Recommendations_OfferRecommendationId",
                table: "Offers",
                column: "OfferRecommendationId",
                principalTable: "Recommendations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
