using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace SMT.Access.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {
        }

        public override void Delete(Department entity)
        {
            using var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = $"delete from departments where hierarchyid.IsDescendantOf('{entity.HierarchyId}') = 1";
            if (cmd.Connection.State != System.Data.ConnectionState.Open) cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }

        public async Task<string> GetByHierarchyIdsync(string departmentId, int level)
        {
            var result = string.Empty;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = $"SELECT dbo.GetDepartmentAsJson('{departmentId}',{level})";
                if (cmd.Connection.State != System.Data.ConnectionState.Open) await cmd.Connection.OpenAsync();
                using(var reader = await cmd.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        result = reader[0].ToString();
                    }
                }
            }

            return result;
        }
    }
}
