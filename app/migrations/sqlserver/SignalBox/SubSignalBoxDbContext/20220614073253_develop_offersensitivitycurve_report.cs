using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_offersensitivitycurve_report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "sp_OfferSensitivityCurve_create.sql");
            var sql = File.ReadAllText(path);
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_OfferSensitivityCurve");
        }
    }
}
