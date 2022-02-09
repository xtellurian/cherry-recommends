using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_calendar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var script = @"
                SET DATEFIRST 1; -- Monday
                DECLARE @StartDate date = '20200101', 
                @Years     int  = 30;
                ;WITH
                    seq(n)
                    AS
                    (
                                    SELECT 1
                        UNION ALL
                            SELECT n + 1
                            FROM seq
                            WHERE n < DATEDIFF(DAY, @StartDate, DATEADD(YEAR, @Years, @StartDate))
                    ),
                    d(d)
                    AS
                    (
                        SELECT DATEADD(DAY, n - 1, @StartDate)
                        FROM seq
                    ),
                    src
                    AS
                    (
                        SELECT
                            CalendarDate = CONVERT(date,       d),
                            DayOfMonth   = DATEPART(DAY,       d),
                            DayName      = DATENAME(WEEKDAY,   d),
                            Week         = DATEPART(WEEK,      d),
                            ISOWeek      = DATEPART(ISO_WEEK,  d),
                            DayOfWeek    = DATEPART(WEEKDAY,   d),
                            MonthOfYear  = DATEPART(MONTH,     d),
                            MonthName    = DATENAME(MONTH,     d),
                            QuarterOfYear= DATEPART(QUARTER,   d),
                            Year         = DATEPART(YEAR,      d),
                            FirstOfMonth = DATEFROMPARTS(YEAR(d), MONTH(d), 1),
                            LastOfYear   = DATEFROMPARTS(YEAR(d), 12, 31),
                            DayOfYear    = DATEPART(DAYOFYEAR, d)
                        FROM d
                    ),
                    dim
                    AS
                    (
                        SELECT
                            CalendarDate,
                            DayOfMonth,
                            DaySuffix        = CONVERT(char(2), CASE WHEN DayOfMonth / 10 = 1 THEN 'th' ELSE 
                                            CASE RIGHT(DayOfMonth, 1) WHEN '1' THEN 'st' WHEN '2' THEN 'nd' 
                                            WHEN '3' THEN 'rd' ELSE 'th' END END),
                            DayName,
                            DayOfWeek,
                            DayOfWeekInMonth = CONVERT(tinyint, ROW_NUMBER() OVER 
                                            (PARTITION BY FirstOfMonth, DayOfWeek ORDER BY CalendarDate)),
                            DayOfYear,
                            IsWeekend        = CASE WHEN DayOfWeek IN (CASE @@DATEFIRST 
                                            WHEN 1 THEN 6 WHEN 7 THEN 1 END,7) 
                                            THEN 1 ELSE 0 END,
                            Week,
                            ISOWeek,
                            FirstOfWeek      = DATEADD(DAY, 1 - DayOfWeek, CalendarDate),
                            LastOfWeek       = DATEADD(DAY, 6, DATEADD(DAY, 1 - DayOfWeek, CalendarDate)),
                            WeekOfMonth      = CONVERT(tinyint, DENSE_RANK() OVER 
                                            (PARTITION BY Year, MonthOfYear ORDER BY Week)),
                            MonthOfYear,
                            MonthName,
                            FirstOfMonth,
                            LastOfMonth      = MAX(CalendarDate) OVER (PARTITION BY Year, MonthOfYear),
                            FirstOfNextMonth = DATEADD(MONTH, 1, FirstOfMonth),
                            LastOfNextMonth  = DATEADD(DAY, -1, DATEADD(MONTH, 2, FirstOfMonth)),
                            QuarterOfYear,
                            FirstOfQuarter   = MIN(CalendarDate) OVER (PARTITION BY Year, QuarterOfYear),
                            LastOfQuarter    = MAX(CalendarDate) OVER (PARTITION BY Year, QuarterOfYear),
                            Year,
                            ISOYear          = Year - CASE WHEN MonthOfYear = 1 AND ISOWeek > 51 THEN 1 
                                            WHEN MonthOfYear = 12 AND ISOWeek = 1  THEN -1 ELSE 0 END,
                            FirstOfYear      = DATEFROMPARTS(Year, 1,  1),
                            LastOfYear,
                            IsLeapYear          = CONVERT(bit, CASE WHEN (Year % 400 = 0)
                                OR (Year % 4 = 0 AND Year % 100 <> 0) 
                                            THEN 1 ELSE 0 END),
                            Has53Weeks          = CASE WHEN DATEPART(WEEK,     LastOfYear) = 53 THEN 1 ELSE 0 END,
                            Has53ISOWeeks       = CASE WHEN DATEPART(ISO_WEEK, LastOfYear) = 53 THEN 1 ELSE 0 END,
                            MMYYYY              = CONVERT(char(2), CONVERT(char(8), CalendarDate, 101))
                                        + CONVERT(char(4), Year),
                            Style101            = CONVERT(char(10), CalendarDate, 101),
                            Style103            = CONVERT(char(10), CalendarDate, 103),
                            Style112            = CONVERT(char(8),  CalendarDate, 112),
                            Style120            = CONVERT(char(10), CalendarDate, 120)
                        FROM src
                    )
                SELECT *
                INTO [dbo].[Calendar]
                FROM dim
                ORDER BY CalendarDate
                OPTION
                (MAXRECURSION
                0);
                GO
                ALTER TABLE [dbo].[Calendar] ALTER COLUMN CalendarDate date NOT NULL;
                GO
                ALTER TABLE [dbo].[Calendar] ADD CONSTRAINT PK_Calendar PRIMARY KEY CLUSTERED(CalendarDate);
                GO";
            migrationBuilder.Sql(script);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var script = @"DROP TABLE [dbo].[Calendar]";
            migrationBuilder.Sql(script);
        }
    }
}
