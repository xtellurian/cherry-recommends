using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_5079_recommendations_for_businesses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_TrackedUsers_TrackedUserId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_TrackedUsers_TrackedUserId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_TrackedUsers_TrackedUserId",
                table: "ParameterSetRecommendations");

            migrationBuilder.RenameColumn(
                name: "TrackedUserId",
                table: "ParameterSetRecommendations",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_ParameterSetRecommendations_TrackedUserId",
                table: "ParameterSetRecommendations",
                newName: "IX_ParameterSetRecommendations_CustomerId");

            migrationBuilder.RenameColumn(
                name: "TrackedUserId",
                table: "ItemsRecommendations",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsRecommendations_TrackedUserId",
                table: "ItemsRecommendations",
                newName: "IX_ItemsRecommendations_CustomerId");

            migrationBuilder.RenameColumn(
                name: "TrackedUserId",
                table: "InvokationLogEntry",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_InvokationLogEntry_TrackedUserId",
                table: "InvokationLogEntry",
                newName: "IX_InvokationLogEntry_CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "TargetType",
                table: "Recommenders",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "Customer");

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "ParameterSetRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "ItemsRecommendations",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BusinessId",
                table: "InvokationLogEntry",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParameterSetRecommendations_BusinessId",
                table: "ParameterSetRecommendations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemsRecommendations_BusinessId",
                table: "ItemsRecommendations",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_InvokationLogEntry_BusinessId",
                table: "InvokationLogEntry",
                column: "BusinessId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_Businesses_BusinessId",
                table: "InvokationLogEntry",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_TrackedUsers_CustomerId",
                table: "InvokationLogEntry",
                column: "CustomerId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_Businesses_BusinessId",
                table: "ItemsRecommendations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_TrackedUsers_CustomerId",
                table: "ItemsRecommendations",
                column: "CustomerId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_Businesses_BusinessId",
                table: "ParameterSetRecommendations",
                column: "BusinessId",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_TrackedUsers_CustomerId",
                table: "ParameterSetRecommendations",
                column: "CustomerId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_Businesses_BusinessId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_InvokationLogEntry_TrackedUsers_CustomerId",
                table: "InvokationLogEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_Businesses_BusinessId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemsRecommendations_TrackedUsers_CustomerId",
                table: "ItemsRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_Businesses_BusinessId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropForeignKey(
                name: "FK_ParameterSetRecommendations_TrackedUsers_CustomerId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ParameterSetRecommendations_BusinessId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_ItemsRecommendations_BusinessId",
                table: "ItemsRecommendations");

            migrationBuilder.DropIndex(
                name: "IX_InvokationLogEntry_BusinessId",
                table: "InvokationLogEntry");

            migrationBuilder.DropColumn(
                name: "TargetType",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "ParameterSetRecommendations");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "ItemsRecommendations");

            migrationBuilder.DropColumn(
                name: "BusinessId",
                table: "InvokationLogEntry");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ParameterSetRecommendations",
                newName: "TrackedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ParameterSetRecommendations_CustomerId",
                table: "ParameterSetRecommendations",
                newName: "IX_ParameterSetRecommendations_TrackedUserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "ItemsRecommendations",
                newName: "TrackedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsRecommendations_CustomerId",
                table: "ItemsRecommendations",
                newName: "IX_ItemsRecommendations_TrackedUserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "InvokationLogEntry",
                newName: "TrackedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_InvokationLogEntry_CustomerId",
                table: "InvokationLogEntry",
                newName: "IX_InvokationLogEntry_TrackedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvokationLogEntry_TrackedUsers_TrackedUserId",
                table: "InvokationLogEntry",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsRecommendations_TrackedUsers_TrackedUserId",
                table: "ItemsRecommendations",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ParameterSetRecommendations_TrackedUsers_TrackedUserId",
                table: "ParameterSetRecommendations",
                column: "TrackedUserId",
                principalTable: "TrackedUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
