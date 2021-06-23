using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_parameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParameterType = table.Column<string>(type: "TEXT", nullable: false),
                    DefaultValue = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CommonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterSetRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterSetRecommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterSetRecommenders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ParameterBounds = table.Column<string>(type: "TEXT", nullable: true),
                    Arguments = table.Column<string>(type: "TEXT", nullable: true),
                    ScoringUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CommonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterSetRecommenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterParameterSetRecommender",
                columns: table => new
                {
                    ParameterSetRecommendersId = table.Column<long>(type: "INTEGER", nullable: false),
                    ParametersId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterParameterSetRecommender", x => new { x.ParameterSetRecommendersId, x.ParametersId });
                    table.ForeignKey(
                        name: "FK_ParameterParameterSetRecommender_Parameters_ParametersId",
                        column: x => x.ParametersId,
                        principalTable: "Parameters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParameterParameterSetRecommender_ParameterSetRecommenders_ParameterSetRecommendersId",
                        column: x => x.ParameterSetRecommendersId,
                        principalTable: "ParameterSetRecommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParameterParameterSetRecommender_ParametersId",
                table: "ParameterParameterSetRecommender",
                column: "ParametersId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters",
                column: "CommonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParameterParameterSetRecommender");

            migrationBuilder.DropTable(
                name: "ParameterSetRecommendations");

            migrationBuilder.DropTable(
                name: "Parameters");

            migrationBuilder.DropTable(
                name: "ParameterSetRecommenders");
        }
    }
}
