using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_hubspot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                table: "TrackUserSystemMaps");

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "TrackUserSystemMaps",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IntegratedSystemId",
                table: "TrackUserSystemMaps",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cache",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CacheLastRefreshed",
                table: "IntegratedSystems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommonId",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationStatus",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "('NotConfigured')");

            migrationBuilder.AddColumn<string>(
                name: "TokenResponse",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TokenResponseUpdated",
                table: "IntegratedSystems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastExchanged",
                table: "ApiKeys",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalExchanges",
                table: "ApiKeys",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Touchpoints",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    CommonId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Touchpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookReceivers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EndpointId = table.Column<string>(type: "TEXT", nullable: true),
                    SharedSecret = table.Column<string>(type: "TEXT", nullable: true),
                    IntegratedSystemId = table.Column<long>(type: "INTEGER", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false),
                    TrackedUserId = table.Column<long>(type: "INTEGER", nullable: false),
                    TouchpointId = table.Column<long>(type: "INTEGER", nullable: true),
                    Values = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserSystemMaps_UserId_TrackedUserId_IntegratedSystemId",
                table: "TrackUserSystemMaps",
                columns: new[] { "UserId", "TrackedUserId", "IntegratedSystemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_CommonId",
                table: "Touchpoints",
                column: "CommonId",
                unique: true);

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
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WebhookReceivers_IntegratedSystemId",
                table: "WebhookReceivers",
                column: "IntegratedSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                table: "TrackUserSystemMaps",
                column: "IntegratedSystemId",
                principalTable: "IntegratedSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                table: "TrackUserSystemMaps",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropTable(
                name: "TrackedUserTouchpoints");

            migrationBuilder.DropTable(
                name: "WebhookReceivers");

            migrationBuilder.DropTable(
                name: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_TrackUserSystemMaps_UserId_TrackedUserId_IntegratedSystemId",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "Cache",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "CacheLastRefreshed",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "CommonId",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "IntegrationStatus",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "TokenResponse",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "TokenResponseUpdated",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "LastExchanged",
                table: "ApiKeys");

            migrationBuilder.DropColumn(
                name: "TotalExchanges",
                table: "ApiKeys");

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "TrackUserSystemMaps",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "IntegratedSystemId",
                table: "TrackUserSystemMaps",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                table: "TrackUserSystemMaps",
                column: "IntegratedSystemId",
                principalTable: "IntegratedSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                table: "TrackUserSystemMaps",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
