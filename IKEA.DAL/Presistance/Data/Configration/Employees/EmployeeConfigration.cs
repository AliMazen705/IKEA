using IKEA.DAL.Common.Enum;
using IKEA.DAL.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Data.Configration.Employees
{
    public class EmployeeConfigration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).HasColumnType("varchar(50)").IsRequired();
            builder.Property(E => E.Address).HasColumnType("varchar(100)");
            builder.Property(E => E.Salary).HasColumnType("decimal(8,2)");
            builder.Property(E => E.CreatedOn).HasDefaultValueSql("GETUTCDATE()");// time & date
            #region Enum
            builder.Property(E => E.gender).HasConversion(
                (gender) => gender.ToString(),//input in database show string
                (gender)=>(Gender) Enum.Parse(typeof(Gender), gender) //output data base to web show emum
                );
            //----------------------------------------
            builder.Property(E => E.EmployeeType).HasConversion(
             (type) => type.ToString(),//input in database show string
             (type) => (EmployeeType)Enum.Parse(typeof(EmployeeType), type) //output data base to web show emum
             );
            #endregion

        }
    }
}
