using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox
{
    public partial class develop_editable_items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Touchpoints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Recommenders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Recommenders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ListPrice",
                table: "RecommendableItems",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "RecommendableItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Parameters",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "IntegratedSystems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Features",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_CommonId",
                table: "Recommenders",
                column: "CommonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recommenders_CommonId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Touchpoints");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "RecommendableItems");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Features");

            migrationBuilder.AlterColumn<string>(
                name: "CommonId",
                table: "Recommenders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<double>(
                name: "ListPrice",
                table: "RecommendableItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);
        }
    }
}
