using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_arpo_report : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", "sp_ARPO_create.sql");
            var sql = File.ReadAllText(path);
            migrationBuilder.Sql(sql);
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_OfferMeanGrossRevenue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.sp_ARPO");
            migrationBuilder.Sql(develop_offer_meangrossrevenue.sp_OfferMeanGrossRevenue);
        }
    }
}
