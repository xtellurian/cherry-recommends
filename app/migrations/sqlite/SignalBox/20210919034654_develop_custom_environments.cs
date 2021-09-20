using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox
{
    public partial class develop_custom_environments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "TrackedUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "Touchpoints",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "Segments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "Rules",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "Recommenders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "RecommendableItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "Parameters",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "IntegratedSystems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EnvironmentId",
                table: "Features",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrackedUsers_EnvironmentId",
                table: "TrackedUsers",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Touchpoints_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Segments_EnvironmentId",
                table: "Segments",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_EnvironmentId",
                table: "Rules",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommenders_EnvironmentId",
                table: "Recommenders",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendableItems_EnvironmentId",
                table: "RecommendableItems",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Parameters_EnvironmentId",
                table: "Parameters",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegratedSystems_EnvironmentId",
                table: "IntegratedSystems",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Features_EnvironmentId",
                table: "Features",
                column: "EnvironmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_IntegratedSystems_Environments_EnvironmentId",
                table: "IntegratedSystems",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecommendableItems_Environments_EnvironmentId",
                table: "RecommendableItems",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Recommenders_Environments_EnvironmentId",
                table: "Recommenders",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Rules_Environments_EnvironmentId",
                table: "Rules",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Segments_Environments_EnvironmentId",
                table: "Segments",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TrackedUsers_Environments_EnvironmentId",
                table: "TrackedUsers",
                column: "EnvironmentId",
                principalTable: "Environments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Features_Environments_EnvironmentId",
                table: "Features");

            migrationBuilder.DropForeignKey(
                name: "FK_IntegratedSystems_Environments_EnvironmentId",
                table: "IntegratedSystems");

            migrationBuilder.DropForeignKey(
                name: "FK_Parameters_Environments_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropForeignKey(
                name: "FK_RecommendableItems_Environments_EnvironmentId",
                table: "RecommendableItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Recommenders_Environments_EnvironmentId",
                table: "Recommenders");

            migrationBuilder.DropForeignKey(
                name: "FK_Rules_Environments_EnvironmentId",
                table: "Rules");

            migrationBuilder.DropForeignKey(
                name: "FK_Segments_Environments_EnvironmentId",
                table: "Segments");

            migrationBuilder.DropForeignKey(
                name: "FK_Touchpoints_Environments_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropForeignKey(
                name: "FK_TrackedUsers_Environments_EnvironmentId",
                table: "TrackedUsers");

            migrationBuilder.DropTable(
                name: "Environments");

            migrationBuilder.DropIndex(
                name: "IX_TrackedUsers_EnvironmentId",
                table: "TrackedUsers");

            migrationBuilder.DropIndex(
                name: "IX_Touchpoints_EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropIndex(
                name: "IX_Segments_EnvironmentId",
                table: "Segments");

            migrationBuilder.DropIndex(
                name: "IX_Rules_EnvironmentId",
                table: "Rules");

            migrationBuilder.DropIndex(
                name: "IX_Recommenders_EnvironmentId",
                table: "Recommenders");

            migrationBuilder.DropIndex(
                name: "IX_RecommendableItems_EnvironmentId",
                table: "RecommendableItems");

            migrationBuilder.DropIndex(
                name: "IX_Parameters_EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropIndex(
                name: "IX_IntegratedSystems_EnvironmentId",
                table: "IntegratedSystems");

            migrationBuilder.DropIndex(
                name: "IX_Features_EnvironmentId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "TrackedUsers");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Touchpoints");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Segments");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Recommenders");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "RecommendableItems");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Parameters");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "EnvironmentId",
                table: "Features");
        }
    }
}
