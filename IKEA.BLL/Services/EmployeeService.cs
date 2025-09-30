using IKEA.BLL.Common.Services;
using IKEA.BLL.Model.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Presistance.Repositories.Employees;
using IKEA.DAL.Presistance.Unit_Of_Work;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public EmployeeService(IUnitOfWork unitOfWork,IAttachmentService attachmentService
            ) 
           //من جوا اليونت هوصل للريبوزاتري
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async  Task< int> CreateEmployeeAsync(CreatedEmployeeDto employeeDto)
        {
            var employee = new  Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive=employeeDto.IsActive,
                Salary= employeeDto.Salary,
                Email= employeeDto.Email,
                PhoneNumber=employeeDto.PhoneNumber,
                HiringDate= employeeDto.HiringDate,
                gender=employeeDto.Gender,
                EmployeeType=employeeDto.EmployeeType,
               DepartmentId = employeeDto.DepartmentId,
                CreatedBy =1,
                LastModifiedBy=1,
                LastModifiedOn=DateTime.UtcNow,
            };
            if (employeeDto.Image is not null)// not null
            {
                employee.Image=_attachmentService.UploadFile(employeeDto.Image,"Images");
            }
                _unitOfWork.EmployeeRepository.Add(employee);
            return await  _unitOfWork.CompleteAsync();
        }

       
           
        public async Task< bool> DeleteEmployeeAsync(int id)
        {
           var employee= await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if(employee is { }) //=null
            {
                
                _unitOfWork.EmployeeRepository.Delete(employee) ;
              
            }
            return await _unitOfWork.CompleteAsync()>0;
        }

        public async  Task< IEnumerable<EmployeeDto>> GetAllEmployeesAsync(string Search)
        {
            return await  _unitOfWork.EmployeeRepository.GetAllAsQueryable()
                .Where(E => !E.IsDeleted
                && (string.IsNullOrEmpty(Search)
                || E.Name.ToLower().Contains(Search.ToLower())))
                 .Include(E => E.Department) // Include the Department navigation property
                .Select(employee => new EmployeeDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    Gender = employee.gender,
                    employeeType = employee.EmployeeType.ToString(),
                    //  Department=employee.Department.Name,
                    Department = employee.Department.Name ?? "No Department", // Handle null Department



                }).ToListAsync();
                //.ToListAsync;
        }

        public async Task< EmployeeDetailsDto?> GetEmployeeByIdAsync(int id)
        {
            var employee =await _unitOfWork.EmployeeRepository.GetByIdAsync(id);
            if (employee !=null)
              
            return new EmployeeDetailsDto()
                  
            {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HiringDate = employee.HiringDate,
                    //--------
                    Gender = employee.gender,
                    EmployeeType = employee.EmployeeType,
                //------
                //Department = employee.Department.Name,
                Department = employee.Department?.Name ?? "No Department",

                Image = employee.Image

                };
            return null;



        }

        public async Task< int> UpdateEmployeeAsync(UpdatedEmployeeDto employeeDto)
        {
            var employee = new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
             _unitOfWork.EmployeeRepository.Update(employee);
            return await _unitOfWork.CompleteAsync();
        }
        
    }
}
