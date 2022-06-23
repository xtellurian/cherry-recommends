using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_customerevent_constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_EventId_EnvironmentId",
                table: "TrackedUserEvents");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_EventId_EnvironmentId",
                table: "TrackedUserEvents",
                columns: new[] { "EventId", "EnvironmentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_EventId_EnvironmentId",
                table: "TrackedUserEvents");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_EventId_EnvironmentId",
                table: "TrackedUserEvents",
                columns: new[] { "EventId", "EnvironmentId" },
                unique: true,
                filter: "[EventId] IS NOT NULL AND [EnvironmentId] IS NOT NULL");
        }
    }
}
