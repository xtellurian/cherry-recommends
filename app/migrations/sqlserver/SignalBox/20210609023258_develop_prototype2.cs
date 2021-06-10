using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_prototype2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastExchanged",
                table: "ApiKeys",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalExchanges",
                table: "ApiKeys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Touchpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Touchpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookReceivers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EndpointId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SharedSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntegratedSystemId = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookReceivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookReceivers_IntegratedSystems_IntegratedSystemId",
                        column: x => x.IntegratedSystemId,
                        principalTable: "IntegratedSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrackedUserTouchpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<int>(type: "int", nullable: false),
                    TrackedUserId = table.Column<long>(type: "bigint", nullable: true),
                    TouchpointId = table.Column<long>(type: "bigint", nullable: true),
                    Values = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackedUserTouchpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackedUserTouchpoints_Touchpoints_TouchpointId",
                        column: x => x.TouchpointId,
                        principalTable: "Touchpoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackedUserTouchpoints_TrackedUsers_TrackedUserId",
                        column: x => x.TrackedUserId,
                        principalTable: "TrackedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints",
                column: "CommonId",
                unique: true,
                filter: "[CommonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserTouchpoints_TouchpointId",
                table: "TrackedUserTouchpoints",
                column: "TouchpointId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUserTouchpoints_TrackedUserId",
                table: "TrackedUserTouchpoints",
                column: "TrackedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookReceivers_EndpointId",
                table: "WebhookReceivers",
                column: "EndpointId",
                unique: true,
                filter: "[EndpointId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookReceivers_IntegratedSystemId",
                table: "WebhookReceivers",
                column: "IntegratedSystemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrackedUserTouchpoints");

            migrationBuilder.DropTable(
                name: "WebhookReceivers");

            migrationBuilder.DropTable(
                name: "Touchpoints");

            migrationBuilder.DropColumn(
                name: "LastExchanged",
                table: "ApiKeys");

            migrationBuilder.DropColumn(
                name: "TotalExchanges",
                table: "ApiKeys");
        }
    }
}
