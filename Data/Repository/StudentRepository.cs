
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_New.Data.Repository
{
    public class StudentRepository : CollegeRepository<Student>, IStudentRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentRepository(CollegeDBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public Task<List<Student>> GetAllStudentsByFees()
        {
            return null;
        }
    }
}
