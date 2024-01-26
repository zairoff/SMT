using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddReturnedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnedProductRepairs",
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
                    table.PrimaryKey("PK_ReturnedProductRepairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProductRepairs_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReturnedProductRepairs_ReturnedProductTransactions_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProductStores",
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
                    table.PrimaryKey("PK_ReturnedProductStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProductStores_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReturnedProductStores_ReturnedProductTransactions_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProductUtilizes",
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
                    table.PrimaryKey("PK_ReturnedProductUtilizes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProductUtilizes_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ReturnedProductUtilizes_ReturnedProductTransactions_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductRepairs_ModelId",
                table: "ReturnedProductRepairs",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductRepairs_ReturnedProductTransactionId",
                table: "ReturnedProductRepairs",
                column: "ReturnedProductTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductStores_ModelId",
                table: "ReturnedProductStores",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductStores_ReturnedProductTransactionId",
                table: "ReturnedProductStores",
                column: "ReturnedProductTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductUtilizes_ModelId",
                table: "ReturnedProductUtilizes",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnedProductUtilizes_ReturnedProductTransactionId",
                table: "ReturnedProductUtilizes",
                column: "ReturnedProductTransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReturnedProductRepairs");

            migrationBuilder.DropTable(
                name: "ReturnedProductStores");

            migrationBuilder.DropTable(
                name: "ReturnedProductUtilizes");
        }
    }
}
