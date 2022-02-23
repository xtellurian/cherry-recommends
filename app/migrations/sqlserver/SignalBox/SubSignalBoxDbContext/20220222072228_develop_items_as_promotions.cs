using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_items_as_promotions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListPrice",
                table: "RecommendableItems");

            migrationBuilder.AddColumn<string>(
                name: "BenefitType",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Fixed");

            migrationBuilder.AddColumn<double>(
                name: "BenefitValue",
                table: "RecommendableItems",
                type: "float",
                nullable: false,
                defaultValue: 1.0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRedemptions",
                table: "RecommendableItems",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "PromotionType",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Other");

            migrationBuilder.UpdateData(
                table: "RecommendableItems",
                keyColumn: "Id",
                keyValue: -1L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "The default promotion. When this promotion is recommended, no action should be taken.", "Default Promotion" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenefitType",
                table: "RecommendableItems");

            migrationBuilder.DropColumn(
                name: "BenefitValue",
                table: "RecommendableItems");

            migrationBuilder.DropColumn(
                name: "NumberOfRedemptions",
                table: "RecommendableItems");

            migrationBuilder.DropColumn(
                name: "PromotionType",
                table: "RecommendableItems");

            migrationBuilder.AddColumn<double>(
                name: "ListPrice",
                table: "RecommendableItems",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "RecommendableItems",
                keyColumn: "Id",
                keyValue: -1L,
                columns: new[] { "Description", "Name" },
                values: new object[] { "The default recommendable item. When this item is recommended, no action should be taken.", "Default Item" });
        }
    }
}
