using System.ComponentModel.DataAnnotations.Schema;

namespace SMT.Domain
{
    public class Department
    {
        public int Id { get; set; }

        [Column(TypeName = "Ltree")]
        public string Ltree { get; set; }

        public string Name { get; set; }
    }
}
