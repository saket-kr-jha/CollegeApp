using DotNetCore_New.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options):base(options) {
        }
        
        public DbSet<Student> Students {  get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());

        }
    }
}
