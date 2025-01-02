namespace DotNetCore_New.Model
{
    public class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>()
        {
                new Student()
                {
                    StudentId = 1,
                    StudentName = "Saket Jha",
                    StudentEmail = "saketkumar180@gmail.com",
                    StudentPhone = "9177881115",
                },
                new Student()
                {
                    StudentId = 2,
                    StudentName = "Subham singh",
                    StudentEmail = "subhamsingh@gmail.com",
                    StudentPhone = "8437074075",
                },
                new Student()
                {
                    StudentId = 3,
                    StudentName = "Tinku Singh",
                    StudentEmail = "tinkusingh@gmail.com",
                    StudentPhone = "8978246007",
                },
                new Student()
                {
                    StudentId = 4,
                    StudentName = "Arun Togi",
                    StudentEmail = "aruntogi@gmail.com",
                    StudentPhone = "8897534078",
                }
            };
        }
}
