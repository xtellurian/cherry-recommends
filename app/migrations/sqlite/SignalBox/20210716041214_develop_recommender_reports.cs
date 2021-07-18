using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_recommender_reports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserTouchpoints_Touchpoints_TouchpointId",
                table: "TrackedUserTouchpoints");

            migrationBuilder.AlterColumn<long>(
                name: "TouchpointId",
                table: "TrackedUserTouchpoints",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RecommenderTargetVariableValue",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecommenderId = table.Column<long>(type: "INTEGER", nullable: false),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    Start = table.Column<long>(type: "INTEGER", nullable: false),
                    End = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<double>(type: "REAL", nullable: false),
                    ParameterSetRecommenderId = table.Column<long>(type: "INTEGER", nullable: true),
                    ProductRecommenderId = table.Column<long>(type: "INTEGER", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommenderTargetVariableValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommenderTargetVariableValue_ParameterSetRecommenders_ParameterSetRecommenderId",
                        column: x => x.ParameterSetRecommenderId,
                        principalTable: "ParameterSetRecommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommenderTargetVariableValue_ProductRecommenders_ProductRecommenderId",
                        column: x => x.ProductRecommenderId,
                        principalTable: "ProductRecommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommenderTargetVariableValue_ParameterSetRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ParameterSetRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommenderTargetVariableValue_ProductRecommenderId",
                table: "RecommenderTargetVariableValue",
                column: "ProductRecommenderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommenderTargetVariableValue_RecommenderId_Name_Version",
                table: "RecommenderTargetVariableValue",
                columns: new[] { "RecommenderId", "Name", "Version" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations",
                column: "RecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserTouchpoints_Touchpoints_TouchpointId",
                table: "TrackedUserTouchpoints",
                column: "TouchpointId",
                principalTable: "Touchpoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserTouchpoints_Touchpoints_TouchpointId",
                table: "TrackedUserTouchpoints");

            migrationBuilder.DropTable(
                name: "RecommenderTargetVariableValue");

            migrationBuilder.AlterColumn<long>(
                name: "TouchpointId",
                table: "TrackedUserTouchpoints",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_ParameterSetRecommenders_RecommenderId",
                table: "ParameterSetRecommendations",
                column: "RecommenderId",
                principalTable: "ParameterSetRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommendations_ProductRecommenders_RecommenderId",
                table: "ProductRecommendations",
                column: "RecommenderId",
                principalTable: "ProductRecommenders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserTouchpoints_Touchpoints_TouchpointId",
                table: "TrackedUserTouchpoints",
                column: "TouchpointId",
                principalTable: "Touchpoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
