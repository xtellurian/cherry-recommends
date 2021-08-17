using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_clone_a_recommender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FeedbackScore",
                table: "TrackedUserActions",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TrackedUserId",
                table: "Recommendations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_TrackedUserId",
                table: "Recommendations",
                column: "TrackedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recommendations_TrackedUsers_TrackedUserId",
                table: "Recommendations",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recommendations_TrackedUsers_TrackedUserId",
                table: "Recommendations");

            migrationBuilder.DropIndex(
                name: "IX_Recommendations_TrackedUserId",
                table: "Recommendations");

            migrationBuilder.DropColumn(
                name: "FeedbackScore",
                table: "TrackedUserActions");

            migrationBuilder.DropColumn(
                name: "TrackedUserId",
                table: "Recommendations");
        }
    }
}
