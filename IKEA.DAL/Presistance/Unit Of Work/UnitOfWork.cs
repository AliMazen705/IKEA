using IKEA.DAL.Presistance.Data;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Repositories.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Unit_Of_Work
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _dBContext;
  
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                return new EmployeeRepository(_dBContext);
            }
        }
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                return new DepartmentRepository(_dBContext);
            }
        }
                

        public UnitOfWork(ApplicationDBContext dBContext)
        {
            //ask clr for create obj from applicationdbcontext implicitly
          
           
            _dBContext = dBContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dBContext.SaveChangesAsync();
        }

        public async  ValueTask DisposeAsync() //void =>valueTask
        {
          await _dBContext.DisposeAsync();
        }

        
    }
}
