using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Model.Departments
{
    public class DepartmentToReturnDTO
    {
        public int Id { get; set; } // Unique identifier for the model
      //  public int CreatedBy { get; set; } // This could be a user ID or system identifierمين عدل مسح اد
     //   public DateTime CreatedOn { get; set; } // Timestamp for when the model was created
      //  public int LastModifiedBy { get; set; } // This could be a user ID or system identifier for the last modifier
      //  public DateTime LastModifiedOn { get; set; } // Timestamp for when the model was last modified
       //  public bool IsDeleted { get; set; } //
        public String Name { get; set; } = null!; // Name of the department, cannot be null مجرد تسامح مع الكوبيلر

       
        public string Code { get; set; } = null!;
        [Display(Name="Date Of Creation")]
        public DateOnly CreationDate { get; set; }
    }
}
