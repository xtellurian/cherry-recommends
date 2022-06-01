using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_deferreddeliverysending : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastAttemptedDelivery",
                table: "DeferredDeliveries");

            migrationBuilder.AddColumn<bool>(
                name: "Sending",
                table: "DeferredDeliveries",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sending",
                table: "DeferredDeliveries");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastAttemptedDelivery",
                table: "DeferredDeliveries",
                type: "datetimeoffset",
                nullable: true);
        }
    }
}
