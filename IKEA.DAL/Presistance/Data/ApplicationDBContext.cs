using IKEA.DAL.Models.Departments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using IKEA.DAL.Models.Employees;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;
using IKEA.DAL.Models.Identity;//flag to use reflection for assembly scanning

namespace IKEA.DAL.Presistance.Data
{
    public class ApplicationDBContext:IdentityDbContext<ApplicationUser>
    {
       public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
          
        }
       
        #region Dbset
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
       
        #endregion
    }
}
