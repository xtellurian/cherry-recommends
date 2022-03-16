using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_predicates_data : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrolmentRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SegmentId = table.Column<long>(type: "bigint", nullable: false),
                    MetricId = table.Column<long>(type: "bigint", nullable: true),
                    NumericPredicate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrolmentRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrolmentRules_Features_MetricId",
                        column: x => x.MetricId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnrolmentRules_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Segments",
                columns: new[] { "Id", "EnvironmentId", "Name" },
                values: new object[] { 100L, null, "More than 10 Events" });

            migrationBuilder.InsertData(
                table: "EnrolmentRules",
                columns: new[] { "Id", "Discriminator", "MetricId", "NumericPredicate", "SegmentId" },
                values: new object[] { 100L, "MetricEnrolmentRule", 101L, "{\"PredicateOperator\":3,\"CompareTo\":10}", 100L });

            migrationBuilder.CreateIndex(
                name: "IX_EnrolmentRules_MetricId",
                table: "EnrolmentRules",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrolmentRules_SegmentId",
                table: "EnrolmentRules",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrolmentRules");

            migrationBuilder.DeleteData(
                table: "Segments",
                keyColumn: "Id",
                keyValue: 100L);
        }
    }
}
