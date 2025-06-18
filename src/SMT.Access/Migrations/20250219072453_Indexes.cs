using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_LineId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ModelId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_BoardReports_QrReaderId",
                table: "BoardReports");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Reports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SapCode",
                table: "Models",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Defects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QrCode",
                table: "BoardReports",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Barcode_Status",
                table: "Reports",
                columns: new[] { "Barcode", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CreatedDate_Status",
                table: "Reports",
                columns: new[] { "CreatedDate", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LineId_Status_CreatedDate",
                table: "Reports",
                columns: new[] { "LineId", "Status", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ModelId_LineId_DefectId_CreatedDate",
                table: "Reports",
                columns: new[] { "ModelId", "LineId", "DefectId", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ModelId_LineId_Status_CreatedDate",
                table: "Reports",
                columns: new[] { "ModelId", "LineId", "Status", "CreatedDate" });

            migrationBuilder.CreateIndex(
                name: "IX_ReadyProductTransactions_Date",
                table: "ReadyProductTransactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyProductTransactions_Date_Status",
                table: "ReadyProductTransactions",
                columns: new[] { "Date", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_QrReaders_Position",
                table: "QrReaders",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_Models_SapCode",
                table: "Models",
                column: "SapCode");

            migrationBuilder.CreateIndex(
                name: "IX_Defects_Name",
                table: "Defects",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BoardReports_DateTime",
                table: "BoardReports",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_BoardReports_QrCode",
                table: "BoardReports",
                column: "QrCode");

            migrationBuilder.CreateIndex(
                name: "IX_BoardReports_QrReaderId_DateTime",
                table: "BoardReports",
                columns: new[] { "QrReaderId", "DateTime" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reports_Barcode_Status",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CreatedDate_Status",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_LineId_Status_CreatedDate",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ModelId_LineId_DefectId_CreatedDate",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ModelId_LineId_Status_CreatedDate",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_ReadyProductTransactions_Date",
                table: "ReadyProductTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ReadyProductTransactions_Date_Status",
                table: "ReadyProductTransactions");

            migrationBuilder.DropIndex(
                name: "IX_QrReaders_Position",
                table: "QrReaders");

            migrationBuilder.DropIndex(
                name: "IX_Models_SapCode",
                table: "Models");

            migrationBuilder.DropIndex(
                name: "IX_Defects_Name",
                table: "Defects");

            migrationBuilder.DropIndex(
                name: "IX_BoardReports_DateTime",
                table: "BoardReports");

            migrationBuilder.DropIndex(
                name: "IX_BoardReports_QrCode",
                table: "BoardReports");

            migrationBuilder.DropIndex(
                name: "IX_BoardReports_QrReaderId_DateTime",
                table: "BoardReports");

            migrationBuilder.AlterColumn<string>(
                name: "Barcode",
                table: "Reports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SapCode",
                table: "Models",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Defects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "QrCode",
                table: "BoardReports",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_LineId",
                table: "Reports",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ModelId",
                table: "Reports",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_BoardReports_QrReaderId",
                table: "BoardReports",
                column: "QrReaderId");
        }
    }
}
