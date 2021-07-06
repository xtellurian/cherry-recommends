using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_product_recommenders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers");

            migrationBuilder.DropIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_Products_CommonId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters");

            migrationBuilder.AlterColumn<string>(
                name: "CommonUserId",
                table: "TrackedUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Touchpoints",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelInput",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelInputType",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelOutput",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelOutputType",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Recommendations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "ParameterSetRecommenders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Parameters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RecommendationCorrelatorId = table.Column<long>(type: "bigint", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelInput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelInputType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelOutput = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelOutputType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecommendations_RecommendationCorrelators_RecommendationCorrelatorId",
                        column: x => x.RecommendationCorrelatorId,
                        principalTable: "RecommendationCorrelators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecommenders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelRegistrationId = table.Column<long>(type: "bigint", nullable: true),
                    TouchpointId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecommenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecommenders_ModelRegistrations_ModelRegistrationId",
                        column: x => x.ModelRegistrationId,
                        principalTable: "ModelRegistrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRecommenders_Touchpoints_TouchpointId",
                        column: x => x.TouchpointId,
                        principalTable: "Touchpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductProductRecommender",
                columns: table => new
                {
                    ProductRecommendersId = table.Column<long>(type: "bigint", nullable: false),
                    ProductsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProductRecommender", x => new { x.ProductRecommendersId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_ProductProductRecommender_ProductRecommenders_ProductRecommendersId",
                        column: x => x.ProductRecommendersId,
                        principalTable: "ProductRecommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductProductRecommender_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers",
                column: "CommonUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CommonId",
                table: "Products",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductProductRecommender_ProductsId",
                table: "ProductProductRecommender",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommendations_RecommendationCorrelatorId",
                table: "ProductRecommendations",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommenders_ModelRegistrationId",
                table: "ProductRecommenders",
                column: "ModelRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommenders_TouchpointId",
                table: "ProductRecommenders",
                column: "TouchpointId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProductRecommender");

            migrationBuilder.DropTable(
                name: "ProductRecommendations");

            migrationBuilder.DropTable(
                name: "ProductRecommenders");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers");

            migrationBuilder.DropIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_Products_CommonId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "ModelInput",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "ModelInputType",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "ModelOutput",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "ModelOutputType",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Recommendations");

            migrationBuilder.AlterColumn<string>(
                name: "CommonUserId",
                table: "TrackedUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Touchpoints",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "ParameterSetRecommenders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Parameters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers",
                column: "CommonUserId",
                unique: true,
                filter: "[CommonUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints",
                column: "CommonId",
                unique: true,
                filter: "[CommonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CommonId",
                table: "Products",
                column: "CommonId",
                unique: true,
                filter: "[CommonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters",
                column: "CommonId",
                unique: true,
                filter: "[CommonId] IS NOT NULL");
        }
    }
}
