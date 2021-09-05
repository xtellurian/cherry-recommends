using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_editable_items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Touchpoints",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Recommenders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ListPrice",
                table: "RecommendableItems",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "RecommendableItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Parameters",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Features",
                type: "TEXT",
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

            migrationBuilder.AlterColumn<double>(
                name: "ListPrice",
                table: "RecommendableItems",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);
        }
    }
}
