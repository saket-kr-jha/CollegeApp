using DotNetCore_New.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options):base(options) {
        }
        
        public DbSet<Student> Students {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentConfig());
        }
    }
}
