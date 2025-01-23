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
        public DbSet<Role> Roles { get; set; }
        public DbSet<RolePrivilage> RolePrivilages { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
        public DbSet<UserType> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new DepartmentConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new RolePrivilageConfig());
            modelBuilder.ApplyConfiguration(new UserRoleMappingConfig());
            modelBuilder.ApplyConfiguration(new UserTypeConfig());
        }
    }
}
