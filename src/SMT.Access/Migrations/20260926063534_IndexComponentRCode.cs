using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class IndexComponentRCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RCode",
                table: "Components",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Components_RCode",
                table: "Components",
                column: "RCode");

            // PartNumber is stored as a JSON array (nvarchar(max)), which SQL Server cannot index
            // directly. PrimaryPartNumber mirrors JSON_VALUE(PartNumber, '$[0]') - the value component
            // code lookups already match on - as a persisted, indexable column so those lookups can use
            // an index instead of scanning every row.
            migrationBuilder.Sql(
                "ALTER TABLE Components ADD PrimaryPartNumber AS CONVERT(nvarchar(450), JSON_VALUE(PartNumber, '$[0]')) PERSISTED;");

            migrationBuilder.Sql(
                "CREATE NONCLUSTERED INDEX IX_Components_PrimaryPartNumber ON Components(PrimaryPartNumber);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "DROP INDEX IX_Components_PrimaryPartNumber ON Components;");

            migrationBuilder.Sql(
                "ALTER TABLE Components DROP COLUMN PrimaryPartNumber;");

            migrationBuilder.DropIndex(
                name: "IX_Components_RCode",
                table: "Components");

            migrationBuilder.AlterColumn<string>(
                name: "RCode",
                table: "Components",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);
        }
    }
}
