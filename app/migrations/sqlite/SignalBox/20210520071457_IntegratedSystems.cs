using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
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
                newName: "Properties");

            migrationBuilder.RenameColumn(
                name: "TrackedUserExternalId",
                table: "Recommendations",
                newName: "CommonUserId");

            migrationBuilder.AddColumn<string>(
                name: "CommonUserId",
                table: "TrackedUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IntegratedSystems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SystemType = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegratedSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelRegistrations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModelType = table.Column<string>(type: "TEXT", nullable: false),
                    HostingType = table.Column<string>(type: "TEXT", nullable: false),
                    ScoringUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Key = table.Column<string>(type: "TEXT", nullable: true),
                    Swagger = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelRegistrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrackedUserEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CommonUserId = table.Column<string>(type: "TEXT", nullable: true),
                    EventId = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    SourceId = table.Column<long>(type: "INTEGER", nullable: true),
                    Kind = table.Column<string>(type: "TEXT", nullable: true),
                    EventType = table.Column<string>(type: "TEXT", nullable: false),
                    Properties = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    IntegratedSystemId = table.Column<long>(type: "INTEGER", nullable: true),
                    TrackedUserId = table.Column<long>(type: "INTEGER", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_EventId",
                table: "TrackedUserEvents",
                column: "EventId",
                unique: true);

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
                name: "CommonUserId",
                table: "TrackedUsers");

            migrationBuilder.RenameColumn(
                name: "Properties",
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
                unique: true);
        }
    }
}
