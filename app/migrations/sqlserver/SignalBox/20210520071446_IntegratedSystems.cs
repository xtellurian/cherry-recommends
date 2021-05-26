using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class IntegratedSystems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_ExternalId",
                table: "TrackedUsers");

            migrationBuilder.RenameColumn(
                name: "ExternalId",
                table: "TrackedUsers",
                newName: "CommonUserId");

            migrationBuilder.RenameColumn(
                name: "TrackedUserExternalId",
                table: "Recommendations",
                newName: "CommonUserId");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "TrackedUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IntegratedSystems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegratedSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelRegistrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HostingType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoringUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Swagger = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelRegistrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackedUserEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SourceId = table.Column<long>(type: "bigint", nullable: true),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUserEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedUserEvents_IntegratedSystems_SourceId",
                        column: x => x.SourceId,
                        principalTable: "IntegratedSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackUserSystemMaps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IntegratedSystemId = table.Column<long>(type: "bigint", nullable: true),
                    TrackedUserId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackUserSystemMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                        column: x => x.IntegratedSystemId,
                        principalTable: "IntegratedSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers",
                column: "CommonUserId",
                unique: true,
                filter: "[CommonUserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_EventId",
                table: "TrackedUserEvents",
                column: "EventId",
                unique: true,
                filter: "[EventId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_SourceId",
                table: "TrackedUserEvents",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserSystemMaps_IntegratedSystemId",
                table: "TrackUserSystemMaps",
                column: "IntegratedSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserSystemMaps_TrackedUserId",
                table: "TrackUserSystemMaps",
                column: "TrackedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelRegistrations");

            migrationBuilder.DropTable(
                name: "TrackedUserEvents");

            migrationBuilder.DropTable(
                name: "TrackUserSystemMaps");

            migrationBuilder.DropTable(
                name: "IntegratedSystems");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "TrackedUsers");

            migrationBuilder.RenameColumn(
                name: "CommonUserId",
                table: "TrackedUsers",
                newName: "ExternalId");

            migrationBuilder.RenameColumn(
                name: "CommonUserId",
                table: "Recommendations",
                newName: "TrackedUserExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_ExternalId",
                table: "TrackedUsers",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");
        }
    }
}
