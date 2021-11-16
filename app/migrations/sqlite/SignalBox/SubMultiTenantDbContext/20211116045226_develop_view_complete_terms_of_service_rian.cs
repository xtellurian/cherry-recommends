using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlite.SignalBox.SubMultiTenantDbContext
{
    public partial class develop_view_complete_terms_of_service_rian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TenantTermsOfServiceAcceptance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TenantId = table.Column<long>(type: "INTEGER", nullable: true),
                    Version = table.Column<string>(type: "TEXT", nullable: false),
                    AcceptedByUserId = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    LastUpdated = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantTermsOfServiceAcceptance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenantTermsOfServiceAcceptance_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TenantTermsOfServiceAcceptance_TenantId",
                table: "TenantTermsOfServiceAcceptance",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantTermsOfServiceAcceptance");
        }
    }
}
