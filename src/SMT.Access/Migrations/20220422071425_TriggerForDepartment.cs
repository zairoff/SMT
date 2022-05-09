using Microsoft.EntityFrameworkCore.Migrations;

namespace SMT.Access.Migrations
{
    public partial class TriggerForDepartment : Migration
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

            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION dbo.GetDepartmentAsJson(@departmentId hierarchyid, @level int)
                RETURNS nvarchar(max)
                AS
                BEGIN
                DECLARE @Json NVARCHAR(MAX) = '{}',
                    @id int,
                    @name varchar(50),
                    @hierar Hierarchyid

                    SET @Json = (SELECT
                    t.Id as id,
                    t.HierarchyId as hierarchyid,
                    t.Name as name,
                    children = JSON_QUERY(dbo.GetDepartmentAsJson(t.HierarchyId, @level + 1))
                    FROM Departments t
                    WHERE t.HierarchyId <> @departmentId
                    AND t.HierarchyId.IsDescendantOf(@departmentId) = 1 and t.HierarchyId.GetLevel() =  @level + 1
                    FOR JSON PATH);

                    IF(@level = 0) 
                    BEGIN
                        SELECT
                        @id = t.Id,
                        @hierar = t.HierarchyId,
                        @name = t.Name
                        FROM Departments t
                        WHERE t.HierarchyId = @departmentId;

                        IF(@Json IS NULL OR @Json = '{}')
                        BEGIN
                            SET @Json = 
                            '""{id"":""' + CONVERT(varchar(7), @Id) +
                            '"",""hierarchyid"":""' + @Hierar.ToString() +
                            '"",""name"":""' + @Name +
                            '"",""children"":[]}';
                            END
                            ELSE
                        BEGIN
                            SET @Json =
                            '""{""id"":""' + CONVERT(varchar(7), @Id) +
                            '"",""hierarchyid"":""' + @Hierar.ToString() +
                            '"",""name"":""' + @Name +
                            '"",""children"":""' + CAST(@Json AS NVARCHAR(MAX)) + '}';
                            END
                        END
                    return @Json;

                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER departments_update_Trigger");
        }
    }
}
