using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class ReturnedProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReturnedProductRepairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedProductionTransactionId = table.Column<int>(type: "int", nullable: false),
                    ReturnedProductTransactionId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnedProductRepairs_ReturnedProductRepairs_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductRepairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProductTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnedProductTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnedProductTransactions_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProductStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedProductionTransactionId = table.Column<int>(type: "int", nullable: false),
                    ReturnedProductTransactionId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnedProductStores_ReturnedProductRepairs_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductRepairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReturnedProductUtilizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnedProductionTransactionId = table.Column<int>(type: "int", nullable: false),
                    ReturnedProductTransactionId = table.Column<int>(type: "int", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReturnedProductUtilizes_ReturnedProductRepairs_ReturnedProductTransactionId",
                        column: x => x.ReturnedProductTransactionId,
                        principalTable: "ReturnedProductRepairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_ReturnedProductTransactions_ModelId",
                table: "ReturnedProductTransactions",
                column: "ModelId");

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
                name: "ReturnedProductStores");

            migrationBuilder.DropTable(
                name: "ReturnedProductTransactions");

            migrationBuilder.DropTable(
                name: "ReturnedProductUtilizes");

            migrationBuilder.DropTable(
                name: "ReturnedProductRepairs");
        }
    }
}
