using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_environment_scoped_customers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_EventId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_CommonId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_RecommendableItems_CommonId",
                table: "RecommendableItems");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_Features_CommonId",
                table: "Features");

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "TrackedUserEvents",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers",
                column: "CommonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_CommonUserId_EnvironmentId",
                table: "TrackedUsers",
                columns: new[] { "CommonUserId", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_EnvironmentId",
                table: "TrackedUserEvents",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserEvents_EventId_EnvironmentId",
                table: "TrackedUserEvents",
                columns: new[] { "EventId", "EnvironmentId" },
                unique: true,
                filter: "[EventId] IS NOT NULL AND [EnvironmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints",
                column: "CommonId");

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId_EnvironmentId",
                table: "Touchpoints",
                columns: new[] { "CommonId", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_CommonId",
                table: "Recommenders",
                column: "CommonId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_CommonId_EnvironmentId",
                table: "Recommenders",
                columns: new[] { "CommonId", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendableItems_CommonId",
                table: "RecommendableItems",
                column: "CommonId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendableItems_CommonId_EnvironmentId",
                table: "RecommendableItems",
                columns: new[] { "CommonId", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters",
                column: "CommonId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CommonId_EnvironmentId",
                table: "Parameters",
                columns: new[] { "CommonId", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_CommonId",
                table: "Features",
                column: "CommonId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_CommonId_EnvironmentId",
                table: "Features",
                columns: new[] { "CommonId", "EnvironmentId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserEvents_Environments_EnvironmentId",
                table: "TrackedUserEvents",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserEvents_Environments_EnvironmentId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_CommonUserId",
                table: "TrackedUsers");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_CommonUserId_EnvironmentId",
                table: "TrackedUsers");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_EnvironmentId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUserEvents_EventId_EnvironmentId",
                table: "TrackedUserEvents");

            migrationBuilder.DropIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_Touchpoints_CommonId_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_CommonId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_CommonId_EnvironmentId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_RecommendableItems_CommonId",
                table: "RecommendableItems");

            migrationBuilder.DropIndex(
                name: "IX_RecommendableItems_CommonId_EnvironmentId",
                table: "RecommendableItems");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_CommonId_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_Features_CommonId",
                table: "Features");

            migrationBuilder.DropIndex(
                name: "IX_Features_CommonId_EnvironmentId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "TrackedUserEvents");

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EnvironmentId = table.Column<long>(type: "bigint", nullable: true),
                    EventKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventLogicalValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SegmentId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rules_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
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
                unique: true,
                filter: "[EventId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_CommonId",
                table: "Recommenders",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecommendableItems_CommonId",
                table: "RecommendableItems",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_CommonId",
                table: "Parameters",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Features_CommonId",
                table: "Features",
                column: "CommonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rules_EnvironmentId",
                table: "Rules",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
