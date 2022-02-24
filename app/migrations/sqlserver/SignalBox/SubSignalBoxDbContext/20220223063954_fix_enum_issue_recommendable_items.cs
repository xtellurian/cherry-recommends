using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class fix_enum_issue_recommendable_items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PromotionType",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Other");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldDefaultValue: "Product");

            migrationBuilder.AlterColumn<string>(
                name: "BenefitType",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Fixed");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PromotionType",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Other",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Discriminator",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Product",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BenefitType",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Fixed",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
