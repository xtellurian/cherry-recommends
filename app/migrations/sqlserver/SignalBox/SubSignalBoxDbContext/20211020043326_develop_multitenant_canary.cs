using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_multitenant_canary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebhookReceivers_IntegratedSystems_IntegratedSystemId",
                table: "WebhookReceivers");

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookReceivers_IntegratedSystems_IntegratedSystemId",
                table: "WebhookReceivers",
                column: "IntegratedSystemId",
                principalTable: "IntegratedSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WebhookReceivers_IntegratedSystems_IntegratedSystemId",
                table: "WebhookReceivers");

            migrationBuilder.AddForeignKey(
                name: "FK_WebhookReceivers_IntegratedSystems_IntegratedSystemId",
                table: "WebhookReceivers",
                column: "IntegratedSystemId",
                principalTable: "IntegratedSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
