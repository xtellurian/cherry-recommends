using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_anonymoususer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RecommendableItems",
                columns: new[] { "Id", "CommonId", "Description", "DirectCost", "Discriminator", "EnvironmentId", "ListPrice", "Name", "Properties" },
                values: new object[] { -1L, "default", "The default recommendable item. When this item is recommended, no action should be taken.", null, "RecommendableItem", null, null, "Default Item", "{}" });

            migrationBuilder.InsertData(
                table: "TrackedUsers",
                columns: new[] { "Id", "CommonUserId", "EnvironmentId", "Name", "Properties" },
                values: new object[] { -1L, "anonymous", null, "Anonymous Customer", "{}" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecommendableItems",
                keyColumn: "Id",
                keyValue: -1L);

            migrationBuilder.DeleteData(
                table: "TrackedUsers",
                keyColumn: "Id",
                keyValue: -1L);
        }
    }
}
