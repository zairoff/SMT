using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddBoardFlowV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QrReadersV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrReadersV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrReadersV2_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardsV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QrCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    CurrentQrReaderId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardsV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardsV2_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardsV2_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BoardsV2_QrReadersV2_CurrentQrReaderId",
                        column: x => x.CurrentQrReaderId,
                        principalTable: "QrReadersV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QrReaderV2Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromReaderId = table.Column<int>(type: "int", nullable: false),
                    ToReaderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QrReaderV2Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QrReaderV2Links_QrReadersV2_FromReaderId",
                        column: x => x.FromReaderId,
                        principalTable: "QrReadersV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QrReaderV2Links_QrReadersV2_ToReaderId",
                        column: x => x.ToReaderId,
                        principalTable: "QrReadersV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BoardMovementsV2",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoardId = table.Column<int>(type: "int", nullable: false),
                    QrReaderId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardMovementsV2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BoardMovementsV2_BoardsV2_BoardId",
                        column: x => x.BoardId,
                        principalTable: "BoardsV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardMovementsV2_QrReadersV2_QrReaderId",
                        column: x => x.QrReaderId,
                        principalTable: "QrReadersV2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMovementsV2_BoardId_DateTime",
                table: "BoardMovementsV2",
                columns: new[] { "BoardId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardMovementsV2_QrReaderId_DateTime",
                table: "BoardMovementsV2",
                columns: new[] { "QrReaderId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardsV2_CurrentQrReaderId_Status",
                table: "BoardsV2",
                columns: new[] { "CurrentQrReaderId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardsV2_LineId_Status",
                table: "BoardsV2",
                columns: new[] { "LineId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BoardsV2_ModelId",
                table: "BoardsV2",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardsV2_QrCode",
                table: "BoardsV2",
                column: "QrCode",
                unique: true,
                filter: "[QrCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_QrReadersV2_LineId_Position",
                table: "QrReadersV2",
                columns: new[] { "LineId", "Position" });

            migrationBuilder.CreateIndex(
                name: "IX_QrReaderV2Links_FromReaderId_ToReaderId",
                table: "QrReaderV2Links",
                columns: new[] { "FromReaderId", "ToReaderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QrReaderV2Links_ToReaderId",
                table: "QrReaderV2Links",
                column: "ToReaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoardMovementsV2");

            migrationBuilder.DropTable(
                name: "QrReaderV2Links");

            migrationBuilder.DropTable(
                name: "BoardsV2");

            migrationBuilder.DropTable(
                name: "QrReadersV2");
        }
    }
}
