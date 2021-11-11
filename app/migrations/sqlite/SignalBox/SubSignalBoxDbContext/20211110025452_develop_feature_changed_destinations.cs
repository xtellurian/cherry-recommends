using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_feature_changed_destinations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureDestinations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeatureId = table.Column<long>(type: "INTEGER", nullable: false),
                    ConnectedSystemId = table.Column<long>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    HubspotPropertyName = table.Column<string>(type: "TEXT", nullable: true),
                    Endpoint = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureDestinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeatureDestinations_Features_FeatureId",
                        column: x => x.FeatureId,
                        principalTable: "Features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureDestinations_IntegratedSystems_ConnectedSystemId",
                        column: x => x.ConnectedSystemId,
                        principalTable: "IntegratedSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureDestinations_ConnectedSystemId",
                table: "FeatureDestinations",
                column: "ConnectedSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureDestinations_FeatureId",
                table: "FeatureDestinations",
                column: "FeatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureDestinations");
        }
    }
}
