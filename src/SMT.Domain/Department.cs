using Microsoft.EntityFrameworkCore;

namespace SMT.Domain
{
    public class Department
    {
        public int Id { get; set; }
        
        public HierarchyId HierarchyId { get; set; }

        public string Name { get; set; }
    }
}
