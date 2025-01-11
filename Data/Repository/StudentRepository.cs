
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentRepository(CollegeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateAsync(Student student)
        {
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return student.StudentId;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
            
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Student>> GetAllAsync()
        {
           return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id, bool useNotracking = false)
        {
            if (useNotracking) {
                return await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(student => student.StudentId == id);
            } else
            return await _dbContext.Students.FirstOrDefaultAsync(student => student.StudentId == id);
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(student => student.StudentName.ToLower().Contains(name.ToLower()));
        }

        public async Task<int> UpdateAsync(Student student)
        { 
           _dbContext.Update(student);
            await _dbContext.SaveChangesAsync();
            return student.StudentId;
        }
    }
}
