using IKEA.BLL.Model.Employees;
using IKEA.DAL.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services
{
    public interface IEmployeeService
    {
     Task<IEnumerable<EmployeeDto>>GetAllEmployeesAsync(string Search); //اختياري ""
       Task< EmployeeDetailsDto?> GetEmployeeByIdAsync(int id);
      Task<  int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto);
       Task< int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto);
       Task< bool> DeleteEmployeeAsync(int id);
    }
}
