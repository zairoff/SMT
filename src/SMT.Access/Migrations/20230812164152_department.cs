using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class department : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TRIGGER departments_update_Trigger ON DBO.Departments
                AFTER INSERT
                AS
                Begin
	                Declare @Id int, @Hieararchy Hierarchyid;
	                Select @Id = i.Id, @Hieararchy = i.HierarchyId from inserted i;
	                if(@Hieararchy is null)
	                Begin
		                update Departments set HierarchyId = '/' where Id = @Id;
	                End
                    else
                    Begin
                        update Departments set HierarchyId = @Hieararchy.ToString() +
		                CONVERT(nvarchar(7), @Id) + '/' where Id = @Id;
                    End
                End
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
