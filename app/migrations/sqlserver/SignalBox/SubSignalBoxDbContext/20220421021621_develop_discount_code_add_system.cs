using Microsoft.EntityFrameworkCore.Migrations;

namespace sqlserver.SignalBox.SubSignalBoxDbContext
{
    public partial class develop_discount_code_add_system : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscountCodeIntegratedSystem",
                columns: table => new
                {
                    GeneratedAtId = table.Column<long>(type: "bigint", nullable: false),
                    GeneratedDiscountCodesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeIntegratedSystem", x => new { x.GeneratedAtId, x.GeneratedDiscountCodesId });
                    table.ForeignKey(
                        name: "FK_DiscountCodeIntegratedSystem_DiscountCodes_GeneratedDiscountCodesId",
                        column: x => x.GeneratedDiscountCodesId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountCodeIntegratedSystem_IntegratedSystems_GeneratedAtId",
                        column: x => x.GeneratedAtId,
                        principalTable: "IntegratedSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeIntegratedSystem_GeneratedDiscountCodesId",
                table: "DiscountCodeIntegratedSystem",
                column: "GeneratedDiscountCodesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountCodeIntegratedSystem");
        }
    }
}
