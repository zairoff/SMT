using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class UpdateReadyProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enter",
                table: "ReadyProducts");

            migrationBuilder.DropColumn(
                name: "Exit",
                table: "ReadyProducts");

            migrationBuilder.DropColumn(
                name: "Inside",
                table: "ReadyProducts");

            migrationBuilder.CreateTable(
                name: "ReadyProductTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadyProductTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadyProductTransactions_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReadyProductTransactions_ModelId",
                table: "ReadyProductTransactions",
                column: "ModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadyProductTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "Enter",
                table: "ReadyProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Exit",
                table: "ReadyProducts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Inside",
                table: "ReadyProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
