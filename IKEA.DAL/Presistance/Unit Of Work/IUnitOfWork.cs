using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Unit_Of_Work
{
    public interface IUnitOfWork: IAsyncDisposable //لازم تكون هي كمان اسنك
    {
        public IEmployeeRepository EmployeeRepository { get; }
        public IDepartmentRepository DepartmentRepository { get;  }
       Task< int> CompleteAsync();
        //بغير ف الميثود بس ي سنك ي اسنك
    }
}
