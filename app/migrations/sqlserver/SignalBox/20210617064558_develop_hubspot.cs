using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_hubspot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUserTouchpoints_TrackedUsers_TrackedUserId",
                table: "TrackedUserTouchpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                table: "TrackUserSystemMaps");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrackUserSystemMaps",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "TrackUserSystemMaps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "IntegratedSystemId",
                table: "TrackUserSystemMaps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "TrackedUserTouchpoints",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cache",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CacheLastRefreshed",
                table: "IntegratedSystems",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommonId",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationStatus",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValueSql: "('NotConfigured')");

            migrationBuilder.AddColumn<string>(
                name: "TokenResponse",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TokenResponseUpdated",
                table: "IntegratedSystems",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackUserSystemMaps_UserId_TrackedUserId_IntegratedSystemId",
                table: "TrackUserSystemMaps",
                columns: new[] { "UserId", "TrackedUserId", "IntegratedSystemId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserTouchpoints_TrackedUsers_TrackedUserId",
                table: "TrackedUserTouchpoints",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_TrackedUserTouchpoints_TrackedUsers_TrackedUserId",
                table: "TrackedUserTouchpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_IntegratedSystems_IntegratedSystemId",
                table: "TrackUserSystemMaps");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackUserSystemMaps_TrackedUsers_TrackedUserId",
                table: "TrackUserSystemMaps");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "TrackUserSystemMaps",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "TrackUserSystemMaps",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "IntegratedSystemId",
                table: "TrackUserSystemMaps",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "TrackedUserTouchpoints",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUserTouchpoints_TrackedUsers_TrackedUserId",
                table: "TrackedUserTouchpoints",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
