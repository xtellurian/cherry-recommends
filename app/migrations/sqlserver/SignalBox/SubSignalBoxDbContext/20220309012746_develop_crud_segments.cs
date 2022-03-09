using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_crud_segments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SegmentTrackedUser");

            migrationBuilder.CreateTable(
                name: "CustomerSegments",
                columns: table => new
                {
                    SegmentId = table.Column<long>(type: "bigint", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSegments", x => new { x.CustomerId, x.SegmentId });
                    table.ForeignKey(
                        name: "FK_CustomerSegments_Segments_SegmentId",
                        column: x => x.SegmentId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSegments_TrackedUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSegments_SegmentId",
                table: "CustomerSegments",
                column: "SegmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerSegments");

            migrationBuilder.CreateTable(
                name: "SegmentTrackedUser",
                columns: table => new
                {
                    InSegmentId = table.Column<long>(type: "bigint", nullable: false),
                    SegmentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SegmentTrackedUser", x => new { x.InSegmentId, x.SegmentsId });
                    table.ForeignKey(
                        name: "FK_SegmentTrackedUser_Segments_SegmentsId",
                        column: x => x.SegmentsId,
                        principalTable: "Segments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SegmentTrackedUser_TrackedUsers_InSegmentId",
                        column: x => x.InSegmentId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SegmentTrackedUser_SegmentsId",
                table: "SegmentTrackedUser",
                column: "SegmentsId");
        }
    }
}
