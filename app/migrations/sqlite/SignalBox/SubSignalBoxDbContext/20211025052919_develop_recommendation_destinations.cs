using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_recommendation_destinations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationId",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationSecret",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "IntegratedSystems",
                type: "TEXT",
                nullable: false,
                defaultValue: "IntegratedSystem");

            migrationBuilder.CreateTable(
                name: "RecommendationDestinations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RecommenderId = table.Column<long>(type: "INTEGER", nullable: false),
                    Trigger = table.Column<string>(type: "TEXT", nullable: true),
                    ConnectedSystemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    Endpoint = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendationDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendationDestinations_IntegratedSystems_ConnectedSystemId",
                        column: x => x.ConnectedSystemId,
                        principalTable: "IntegratedSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendationDestinations_Recommenders_RecommenderId",
                        column: x => x.RecommenderId,
                        principalTable: "Recommenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationDestinations_ConnectedSystemId",
                table: "RecommendationDestinations",
                column: "ConnectedSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendationDestinations_RecommenderId",
                table: "RecommendationDestinations",
                column: "RecommenderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendationDestinations");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "ApplicationSecret",
                table: "IntegratedSystems");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "IntegratedSystems");
        }
    }
}
