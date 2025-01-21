﻿namespace DotNetCore_New.Data
{
    public class Department
    {
        public int DepartmentId {  get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
