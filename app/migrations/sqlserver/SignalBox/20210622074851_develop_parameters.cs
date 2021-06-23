using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_parameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parameters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParameterType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parameters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterSetRecommendations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterSetRecommendations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterSetRecommenders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParameterBounds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Arguments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScoringUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParameterSetRecommenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParameterParameterSetRecommender",
                columns: table => new
                {
                    ParameterSetRecommendersId = table.Column<long>(type: "bigint", nullable: false),
                    ParametersId = table.Column<long>(type: "bigint", nullable: false)
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
                unique: true,
                filter: "[CommonId] IS NOT NULL");
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
