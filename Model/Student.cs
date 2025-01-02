using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore_New.Model
{
    public class Student
    {
        [ValidateNever]
        public int StudentId { get; set; }
        [Required]
        [StringLength(100)]
        public string StudentName { get; set; }
        [EmailAddress]
        public string StudentEmail { get; set; }
        public string StudentPhone { get; set; }
        public string StudentAddress { get; set; }
        public string DOB { get; set; } 

    }

}
