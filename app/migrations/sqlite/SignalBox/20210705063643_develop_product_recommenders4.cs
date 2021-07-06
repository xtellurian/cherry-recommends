using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_product_recommenders4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "Created",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(long),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "LastUpdated",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<long>(
                name: "Created",
                table: "ProductRecommendations",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
