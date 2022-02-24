using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_addbusinessentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BusinessMembership_BusinessId",
                table: "TrackedUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Businesses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    EnvironmentId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommonId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Businesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Businesses_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_BusinessMembership_BusinessId",
                table: "TrackedUsers",
                column: "BusinessMembership_BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_CommonId",
                table: "Businesses",
                column: "CommonId");

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_CommonId_EnvironmentId",
                table: "Businesses",
                columns: new[] { "CommonId", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Businesses_EnvironmentId",
                table: "Businesses",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUsers_Businesses_BusinessMembership_BusinessId",
                table: "TrackedUsers",
                column: "BusinessMembership_BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUsers_Businesses_BusinessMembership_BusinessId",
                table: "TrackedUsers");

            migrationBuilder.DropTable(
                name: "Businesses");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_BusinessMembership_BusinessId",
                table: "TrackedUsers");

            migrationBuilder.DropColumn(
                name: "BusinessMembership_BusinessId",
                table: "TrackedUsers");
        }
    }
}
