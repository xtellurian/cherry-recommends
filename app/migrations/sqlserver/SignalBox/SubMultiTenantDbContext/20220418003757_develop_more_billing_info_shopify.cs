using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubMultiTenantDbContext
{
    public partial class develop_more_billing_info_shopify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "Tenants",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BillingAccount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingAccount", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_AccountId",
                table: "Tenants",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_BillingAccount_AccountId",
                table: "Tenants",
                column: "AccountId",
                principalTable: "BillingAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_BillingAccount_AccountId",
                table: "Tenants");

            migrationBuilder.DropTable(
                name: "BillingAccount");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_AccountId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Tenants");
        }
    }
}
