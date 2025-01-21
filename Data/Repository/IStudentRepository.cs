namespace DotNetCore_New.Data.Repository
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
       Task<List<Student>> GetAllStudentsByFees();
    }
}
