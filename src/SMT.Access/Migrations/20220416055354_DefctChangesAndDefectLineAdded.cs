using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class DefctChangesAndDefectLineAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defects_Lines_LineId",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_Defects_LineId",
                table: "Defects");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "Defects");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineId",
                table: "Defects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Defects_LineId",
                table: "Defects",
                column: "LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defects_Lines_LineId",
                table: "Defects",
                column: "LineId",
                principalTable: "Lines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
