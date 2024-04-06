using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class DailyHourlyPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HourlyPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Plan = table.Column<int>(type: "int", nullable: false),
                    Produced = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HourlyPlans_Lines_LineId",
                        column: x => x.LineId,
                        principalTable: "Lines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HourlyPlans_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HourlyPlans_LineId",
                table: "HourlyPlans",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_HourlyPlans_ModelId",
                table: "HourlyPlans",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HourlyPlans");
        }
    }
}
