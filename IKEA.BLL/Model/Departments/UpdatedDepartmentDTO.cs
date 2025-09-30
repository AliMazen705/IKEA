using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Model.Departments
{
    public class UpdatedDepartmentDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!; // Name of the department, cannot be null مجرد تسامح مع الكوبيلر

        public string? Description { get; set; }
        public string Code { get; set; } = null!;

        public DateOnly CreationDate { get; set; }
    }
}
