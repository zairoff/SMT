using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class AddPcbaInstructionDisplay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InstructionPositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructionPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstructionPositions_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LineActiveModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineActiveModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineActiveModels_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineActiveModels_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModelInstructionImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    InstructionPositionId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelInstructionImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModelInstructionImages_InstructionPositions_InstructionPositionId",
                        column: x => x.InstructionPositionId,
                        principalTable: "InstructionPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModelInstructionImages_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstructionPositions_LineId_Order",
                table: "InstructionPositions",
                columns: new[] { "LineId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_LineActiveModels_LineId",
                table: "LineActiveModels",
                column: "LineId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LineActiveModels_ModelId",
                table: "LineActiveModels",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelInstructionImages_InstructionPositionId",
                table: "ModelInstructionImages",
                column: "InstructionPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelInstructionImages_ModelId_InstructionPositionId",
                table: "ModelInstructionImages",
                columns: new[] { "ModelId", "InstructionPositionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineActiveModels");

            migrationBuilder.DropTable(
                name: "ModelInstructionImages");

            migrationBuilder.DropTable(
                name: "InstructionPositions");
        }
    }
}
