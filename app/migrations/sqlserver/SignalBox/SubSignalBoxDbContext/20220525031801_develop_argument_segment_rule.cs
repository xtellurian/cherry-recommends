using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_argument_segment_rule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ArgumentValue",
                table: "ArgumentRules",
                type: "nvarchar(127)",
                maxLength: 127,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SegmentId",
                table: "ArgumentRules",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentRules_ArgumentId_SegmentId_CampaignId_ArgumentValue",
                table: "ArgumentRules",
                columns: new[] { "ArgumentId", "SegmentId", "CampaignId", "ArgumentValue" },
                unique: true,
                filter: "[SegmentId] IS NOT NULL AND [ArgumentValue] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ArgumentRules_SegmentId",
                table: "ArgumentRules",
                column: "SegmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArgumentRules_Segments_SegmentId",
                table: "ArgumentRules",
                column: "SegmentId",
                principalTable: "Segments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArgumentRules_Segments_SegmentId",
                table: "ArgumentRules");

            migrationBuilder.DropIndex(
                name: "IX_ArgumentRules_ArgumentId_SegmentId_CampaignId_ArgumentValue",
                table: "ArgumentRules");

            migrationBuilder.DropIndex(
                name: "IX_ArgumentRules_SegmentId",
                table: "ArgumentRules");

            migrationBuilder.DropColumn(
                name: "SegmentId",
                table: "ArgumentRules");

            migrationBuilder.AlterColumn<string>(
                name: "ArgumentValue",
                table: "ArgumentRules",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(127)",
                oldMaxLength: 127,
                oldNullable: true);
        }
    }
}
