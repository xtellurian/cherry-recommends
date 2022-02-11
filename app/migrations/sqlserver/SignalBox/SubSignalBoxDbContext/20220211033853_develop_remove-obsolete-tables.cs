using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_removeobsoletetables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RewardSelectors");

            migrationBuilder.DropTable(
                name: "TrackedUserActions");

            migrationBuilder.DropTable(
                name: "TrackedUserTouchpoints");

            migrationBuilder.DropTable(
                name: "Touchpoints");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RewardSelectors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    SelectorType = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardSelectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Touchpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EnvironmentId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Touchpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Touchpoints_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrackedUserActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ActionValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssociatedRevenue = table.Column<double>(type: "float", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CommonUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeedbackScore = table.Column<double>(type: "float", nullable: true),
                    IntegratedSystemId = table.Column<long>(type: "bigint", nullable: true),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    RecommendationCorrelatorId = table.Column<long>(type: "bigint", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TrackedUserEventId = table.Column<long>(type: "bigint", nullable: true),
                    TrackedUserId = table.Column<long>(type: "bigint", nullable: true),
                    ValueType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUserActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedUserActions_RecommendationCorrelators_RecommendationCorrelatorId",
                        column: x => x.RecommendationCorrelatorId,
                        principalTable: "RecommendationCorrelators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedUserActions_TrackedUserEvents_TrackedUserEventId",
                        column: x => x.TrackedUserEventId,
                        principalTable: "TrackedUserEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedUserActions_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackedUserTouchpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    TouchpointId = table.Column<long>(type: "bigint", nullable: false),
                    TrackedUserId = table.Column<long>(type: "bigint", nullable: false),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUserTouchpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedUserTouchpoints_Touchpoints_TouchpointId",
                        column: x => x.TouchpointId,
                        principalTable: "Touchpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackedUserTouchpoints_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RewardSelectors_ActionName_SelectorType",
                table: "RewardSelectors",
                columns: new[] { "ActionName", "SelectorType" },
                unique: true,
                filter: "[ActionName] IS NOT NULL");

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
                name: "IX_Touchpoints_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_ActionName",
                table: "TrackedUserActions",
                column: "ActionName");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_CommonUserId",
                table: "TrackedUserActions",
                column: "CommonUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_RecommendationCorrelatorId",
                table: "TrackedUserActions",
                column: "RecommendationCorrelatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_Timestamp",
                table: "TrackedUserActions",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_TrackedUserEventId",
                table: "TrackedUserActions",
                column: "TrackedUserEventId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserActions_TrackedUserId",
                table: "TrackedUserActions",
                column: "TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserTouchpoints_TouchpointId",
                table: "TrackedUserTouchpoints",
                column: "TouchpointId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserTouchpoints_TrackedUserId",
                table: "TrackedUserTouchpoints",
                column: "TrackedUserId");
        }
    }
}
