using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_aggregatemetrics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "HistoricTrackedUserFeatures",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "HistoricCustomerMetric");

            migrationBuilder.AddColumn<string>(
                name: "AggregateCustomerMetric_AggregationType",
                table: "FeatureGenerators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AggregateCustomerMetric_CategoricalValue",
                table: "FeatureGenerators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AggregateCustomerMetric_MetricId",
                table: "FeatureGenerators",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinTwoMetrics_JoinType",
                table: "FeatureGenerators",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "JoinTwoMetrics_Metric1Id",
                table: "FeatureGenerators",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "JoinTwoMetrics_Metric2Id",
                table: "FeatureGenerators",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId",
                table: "HistoricTrackedUserFeatures",
                column: "MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureGenerators_AggregateCustomerMetric_MetricId",
                table: "FeatureGenerators",
                column: "AggregateCustomerMetric_MetricId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureGenerators_JoinTwoMetrics_Metric1Id",
                table: "FeatureGenerators",
                column: "JoinTwoMetrics_Metric1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureGenerators_JoinTwoMetrics_Metric2Id",
                table: "FeatureGenerators",
                column: "JoinTwoMetrics_Metric2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureGenerators_Features_AggregateCustomerMetric_MetricId",
                table: "FeatureGenerators",
                column: "AggregateCustomerMetric_MetricId",
                principalTable: "Features",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureGenerators_Features_JoinTwoMetrics_Metric1Id",
                table: "FeatureGenerators",
                column: "JoinTwoMetrics_Metric1Id",
                principalTable: "Features",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeatureGenerators_Features_JoinTwoMetrics_Metric2Id",
                table: "FeatureGenerators",
                column: "JoinTwoMetrics_Metric2Id",
                principalTable: "Features",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeatureGenerators_Features_AggregateCustomerMetric_MetricId",
                table: "FeatureGenerators");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureGenerators_Features_JoinTwoMetrics_Metric1Id",
                table: "FeatureGenerators");

            migrationBuilder.DropForeignKey(
                name: "FK_FeatureGenerators_Features_JoinTwoMetrics_Metric2Id",
                table: "FeatureGenerators");

            migrationBuilder.DropIndex(
                name: "IX_HistoricTrackedUserFeatures_MetricId",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropIndex(
                name: "IX_FeatureGenerators_AggregateCustomerMetric_MetricId",
                table: "FeatureGenerators");

            migrationBuilder.DropIndex(
                name: "IX_FeatureGenerators_JoinTwoMetrics_Metric1Id",
                table: "FeatureGenerators");

            migrationBuilder.DropIndex(
                name: "IX_FeatureGenerators_JoinTwoMetrics_Metric2Id",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "HistoricTrackedUserFeatures");

            migrationBuilder.DropColumn(
                name: "AggregateCustomerMetric_AggregationType",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "AggregateCustomerMetric_CategoricalValue",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "AggregateCustomerMetric_MetricId",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "JoinTwoMetrics_JoinType",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "JoinTwoMetrics_Metric1Id",
                table: "FeatureGenerators");

            migrationBuilder.DropColumn(
                name: "JoinTwoMetrics_Metric2Id",
                table: "FeatureGenerators");

            migrationBuilder.AlterColumn<long>(
                name: "TrackedUserId",
                table: "HistoricTrackedUserFeatures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
