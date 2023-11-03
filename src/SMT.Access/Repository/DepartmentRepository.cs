using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace SMT.Access.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Department>> GetByAsync(Expression<Func<Department, bool>> expression)
        {
            return await DbSet.Where(expression).OrderBy(x => x.Name).ToListAsync();
        }

        public override void Delete(Department entity)
        {
            using var cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = $"delete from departments where hierarchyid.IsDescendantOf('{entity.HierarchyId}') = 1";
            if (cmd.Connection.State != System.Data.ConnectionState.Open) cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }

        // this method is obsolate
        public async Task<string> GetByHierarchyIdsync(string departmentId, int level)
        {
            var result = string.Empty;
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = $"SELECT dbo.GetDepartmentAsJson('{departmentId}',{level})";
                if (cmd.Connection.State != System.Data.ConnectionState.Open) await cmd.Connection.OpenAsync();
                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    result = reader[0].ToString();
                }
            }

            return result;
        }

        public async Task<IEnumerable<Department>> GetByHierarchyIdsync(string departmentId)
        {
            return await DbSet.Where(d => d.HierarchyId.IsDescendantOf(HierarchyId.Parse(departmentId))).ToListAsync();
        }

        // this object is department response
        /*public async Task<IEnumerable<DepartmentResponse>> GetByHierarchyIdsync(string departmentId)
        {
            var departments = await DbSet.Where(d => d.HierarchyId.IsDescendantOf(HierarchyId.Parse(departmentId)))
                                    .Select(d => new
                                    {
                                        Id = d.Id,
                                        Name = d.Name,
                                        HierarchyId = d.HierarchyId.ToString(),
                                        ParentHierarchyId = d.HierarchyId.GetAncestor(1),
                                        ParentName = DbSet.Where(sub => sub.HierarchyId == d.HierarchyId.GetAncestor(1))
                                                    .Select(sub => sub.Name)
                                                    .FirstOrDefault()
                                    }).ToListAsync();

            return departments;
        }*/
    }
}
