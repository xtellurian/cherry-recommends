using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_calendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create empty calendar table since converting the sql version is quite complex at the moment
            var script = @"
            CREATE TABLE Calendar(
                [CalendarDate] [date] NOT NULL,
                [DayOfMonth] [int] NULL,
                [DaySuffix] [char](2) NULL,
                [DayName] [nvarchar](30) NULL,
                [DayOfWeek] [int] NULL,
                [DayOfWeekInMonth] [tinyint] NULL,
                [DayOfYear] [int] NULL,
                [IsWeekend] [int] NOT NULL,
                [Week] [int] NULL,
                [ISOWeek] [int] NULL,
                [FirstOfWeek] [date] NULL,
                [LastOfWeek] [date] NULL,
                [WeekOfMonth] [tinyint] NULL,
                [MonthOfYear] [int] NULL,
                [MonthName] [nvarchar](30) NULL,
                [FirstOfMonth] [date] NULL,
                [LastOfMonth] [date] NULL,
                [FirstOfNextMonth] [date] NULL,
                [LastOfNextMonth] [date] NULL,
                [QuarterOfYear] [int] NULL,
                [FirstOfQuarter] [date] NULL,
                [LastOfQuarter] [date] NULL,
                [Year] [int] NULL,
                [ISOYear] [int] NULL,
                [FirstOfYear] [date] NULL,
                [LastOfYear] [date] NULL,
                [IsLeapYear] [bit] NULL,
                [Has53Weeks] [int] NOT NULL,
                [Has53ISOWeeks] [int] NOT NULL,
                [MMYYYY] [char](6) NULL,
                [Style101] [char](10) NULL,
                [Style103] [char](10) NULL,
                [Style112] [char](8) NULL,
                [Style120] [char](10) NULL,
                CONSTRAINT [PK_Calendar] PRIMARY KEY 
                (
                    [CalendarDate] ASC
                )
            )";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP TABLE Calendar";
            migrationBuilder.Sql(script);
        }
    }
}
