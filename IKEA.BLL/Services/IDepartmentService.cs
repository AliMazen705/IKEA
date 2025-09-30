using IKEA.BLL.Model.Departments;
using IKEA.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services
{
    public interface IDepartmentService
    {
       Task< IEnumerable<DepartmentToReturnDTO>> GetAllDepartmentsAsync(); //
      Task<  DepartmentDetailsToReturnDTO> GetDepartmentsByIdAsync(int id); //get department by id
       Task< int> CreateDepartmentAsync(CreatedDepartmentDTO departmentDTO);
        Task<int> UpdateDepartmentAsync(UpdatedDepartmentDTO departmentDTO); //update department
       Task< bool> DeleteDepartmentAsync(int id); //delete department by id ف الحذف مش بعل دي تي او
    }
}
