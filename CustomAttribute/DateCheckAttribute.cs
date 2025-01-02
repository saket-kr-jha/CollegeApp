using System.ComponentModel.DataAnnotations;

namespace DotNetCore_New.CustomAttribute
{
    public class DateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;
            if (date < DateTime.Now) {
                return new ValidationResult("The date must be greater than todays date");
            }
            return ValidationResult.Success;
        }
    }
}
