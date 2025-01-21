using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.DepartmentId);
            builder.Property(x => x.DepartmentId).UseIdentityColumn();
            builder.Property(n => n.DepartmentName).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Description).HasMaxLength(500).IsRequired(false);
            builder.HasData(new List<Department>()
            {
                new Department
                {
                    DepartmentId = 1,
                    DepartmentName = "CSE",
                    Description = "Computer Science Engineering",
                },
                new Department
                {
                    DepartmentId = 2,
                    DepartmentName = "ECE",
                    Description = "Electronics and Communication Engineering",
                }
            });

        }
    }
}
