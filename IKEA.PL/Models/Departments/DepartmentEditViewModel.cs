
using System.ComponentModel.DataAnnotations;

namespace IKEA.PL.Models.Departments
{
    public class DepartmentEditViewModel
    {
        public string Name { get; set; } = null!; // Name of the department, cannot be null مجرد تسامح مع الكوبيلر

        public string? Description { get; set; }
        [Required(ErrorMessage ="Code is Required !!!")]
        public string Code { get; set; } = null!;

        public DateOnly CreationDate { get; set; }
    }
}
