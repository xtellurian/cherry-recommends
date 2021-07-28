using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_3837 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DefaultProductId",
                table: "ProductRecommenders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorHandling",
                table: "ProductRecommenders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorHandling",
                table: "ParameterSetRecommenders",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecommenders_DefaultProductId",
                table: "ProductRecommenders",
                column: "DefaultProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRecommenders_Products_DefaultProductId",
                table: "ProductRecommenders",
                column: "DefaultProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRecommenders_Products_DefaultProductId",
                table: "ProductRecommenders");

            migrationBuilder.DropIndex(
                name: "IX_ProductRecommenders_DefaultProductId",
                table: "ProductRecommenders");

            migrationBuilder.DropColumn(
                name: "DefaultProductId",
                table: "ProductRecommenders");

            migrationBuilder.DropColumn(
                name: "ErrorHandling",
                table: "ProductRecommenders");

            migrationBuilder.DropColumn(
                name: "ErrorHandling",
                table: "ParameterSetRecommenders");
        }
    }
}
