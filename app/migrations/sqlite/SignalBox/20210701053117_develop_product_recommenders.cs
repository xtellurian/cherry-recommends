using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_product_recommenders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CommonUserId",
                table: "TrackedUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Touchpoints",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelInput",
                table: "Recommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelInputType",
                table: "Recommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelOutput",
                table: "Recommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelOutputType",
                table: "Recommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Recommendations",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "ParameterSetRecommenders",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Parameters",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false),
                    RecommendationCorrelatorId = table.Column<long>(type: "INTEGER", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: true),
                    ModelInput = table.Column<string>(type: "TEXT", nullable: true),
                    ModelInputType = table.Column<string>(type: "TEXT", nullable: true),
                    ModelOutput = table.Column<string>(type: "TEXT", nullable: true),
                    ModelOutputType = table.Column<string>(type: "TEXT", nullable: true)
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelRegistrationId = table.Column<long>(type: "INTEGER", nullable: true),
                    TouchpointId = table.Column<long>(type: "INTEGER", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CommonId = table.Column<string>(type: "TEXT", nullable: false)
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
                    ProductRecommendersId = table.Column<long>(type: "INTEGER", nullable: false),
                    ProductsId = table.Column<long>(type: "INTEGER", nullable: false)
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
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Touchpoints",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Products",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "ParameterSetRecommenders",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Parameters",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
