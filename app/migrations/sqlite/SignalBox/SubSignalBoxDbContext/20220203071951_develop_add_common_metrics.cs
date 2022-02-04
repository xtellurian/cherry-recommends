using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_add_common_metrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "CommonId", "EnvironmentId", "Name", "Properties" },
                values: new object[] { 100L, "revenue", null, "Revenue", "{}" });

            migrationBuilder.InsertData(
                table: "Features",
                columns: new[] { "Id", "CommonId", "EnvironmentId", "Name", "Properties" },
                values: new object[] { 101L, "total_events", null, "Total Events", "{}" });

            migrationBuilder.InsertData(
                table: "FeatureGenerators",
                columns: new[] { "Id", "FilterSelectAggregateSteps", "GeneratorType", "LastCompleted", "LastEnqueued", "MetricId" },
                values: new object[] { 100L, "[{\"Order\":1,\"Filter\":null,\"Select\":{\"PropertyNameMatch\":null},\"Aggregate\":null},{\"Order\":2,\"Filter\":null,\"Select\":null,\"Aggregate\":{\"AggregationType\":0}}]", "FilterSelectAggregate", null, null, 101L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FeatureGenerators",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DeleteData(
                table: "Features",
                keyColumn: "Id",
                keyValue: 101L);
        }
    }
}
