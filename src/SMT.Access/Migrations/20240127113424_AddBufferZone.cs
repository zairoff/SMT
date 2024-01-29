using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddBufferZone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnedProductBufferZones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedProductTransactionId = table.Column<int>(type: "int", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedProductBufferZones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProductBufferZones_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReturnedProductBufferZones_ReturnedProductTransactions_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductBufferZones_ModelId",
                table: "ReturnedProductBufferZones",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductBufferZones_ReturnedProductTransactionId",
                table: "ReturnedProductBufferZones",
                column: "ReturnedProductTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnedProductBufferZones");
        }
    }
}
