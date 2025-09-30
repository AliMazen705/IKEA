using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Model.Departments
{
    public class CreatedDepartmentDTO
    {
        [Required(ErrorMessage = "Name Is Required !!!")]
        public String Name { get; set; } = null!; // Name of the department, cannot be null مجرد تسامح مع الكوبيلر

        public String? Description { get; set; }
        [Required(ErrorMessage = "Code Is Required !!!")]
        public string Code { get; set; } = null!;
        [Display(Name="Date Of Creation")]
        public DateOnly CreationDate { get; set; }

    }
}
