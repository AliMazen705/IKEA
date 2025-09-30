using IKEA.BLL.Model.Departments;
using IKEA.BLL.Model.Employees;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Presistance.Repositories.Departments;
using IKEA.DAL.Presistance.Unit_Of_Work;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DepartmentToReturnDTO>> GetAllDepartmentsAsync()
        {
            // هرجع كل الداتا بس mapping 
            var departments =await _unitOfWork.DepartmentRepository.GetAllAsQueryable().Select(D => new DepartmentToReturnDTO
            {
                Id = D.Id,
                Name = D.Name,
                Code = D.Code,
                CreationDate = D.CreationDate
            }).AsNoTracking().ToListAsync();
            return departments;
            //to list to execute the query
            //mapping Manull (Auto mapper)
            //foreach (var department in departments)
            //{
            //    yield return new DepartmentToReturnDTO
            //    {
            //        Id = department.Id,
            //        Name = department.Name,
            //        Description = department.Description,
            //        Code = department.Code,
            //        CreationDate = department.CreationDate
            //    };
              

            //}
        }

        public async Task<DepartmentDetailsToReturnDTO?> GetDepartmentsByIdAsync(int id)
        {
            var department =await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is { })
            {
                return new DepartmentDetailsToReturnDTO
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    Code = department.Code,
                    CreationDate = department.CreationDate,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    LastModifiedBy = department.LastModifiedBy,
                    LastModifiedOn = department.LastModifiedOn,


                };
            }
            return null; // or throw an exception if you prefer


        }


        public async Task<int> CreateDepartmentAsync(CreatedDepartmentDTO departmentDTO)
        {
            var Createddepartment = new Department()
            {
                Name = departmentDTO.Name,
                Description = departmentDTO.Description,
                Code = departmentDTO.Code,
                CreationDate = departmentDTO.CreationDate,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
              
            };
             _unitOfWork.DepartmentRepository.Add(Createddepartment);
            return await _unitOfWork.CompleteAsync();

        }


        public async Task<int> UpdateDepartmentAsync(UpdatedDepartmentDTO departmentDTO)
        {
            var UpdatedDepartment = new Department()
            {
             Id= departmentDTO.ID,
                Name = departmentDTO.Name,
                Description = departmentDTO.Description,
                Code = departmentDTO.Code,
                CreationDate = departmentDTO.CreationDate,

                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,

            };
            _unitOfWork.DepartmentRepository.Update(UpdatedDepartment);
            return await _unitOfWork.CompleteAsync();
        }


        public async Task< bool> DeleteDepartmentAsync(int id)
        {
           var department= await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is not null)
            {
                _unitOfWork.DepartmentRepository.Delete(department);
              

            }

            return await _unitOfWork.CompleteAsync() > 0;

        }




    }
}
