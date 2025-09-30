using IKEA.DAL.Models.Departments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Presistance.Data.Configration.Departments
{
    internal class DepartmentConfigrations : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
           builder.Property(D=>D.Id).UseIdentityColumn(10,10);
            builder.Property(D => D.Name).IsRequired().HasColumnType("varchar(100)");
            builder.Property(D => D.Code).IsRequired().HasColumnType("varchar(100)");
            builder.Property(D=>D.CreatedOn).HasDefaultValueSql("GETUTCDATE()"); // Default value for CreatedOn
            builder.Property(D => D.LastModifiedOn).HasDefaultValueSql("GETDATE()"); // Default value for LastModifiedOn

            #region for work relationship
            builder.HasMany(D => D.Employees)
                    .WithOne(E => E.Department)
                    .HasForeignKey(E => E.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull); // Optional: Set foreign key to null if department is deleted

            #endregion
        }
    }
}
