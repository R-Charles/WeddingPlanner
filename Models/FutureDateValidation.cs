using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models;

public class FutureDate : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if(value == null)
        {
            // pass our validator so that it continues to the next one 
            // bc we can't check an empty date
            return ValidationResult.Success;
        }

        DateTime date = (DateTime)value;
        if (date <= DateTime.Now)
        {
            return new ValidationResult("must be in the future");
        }
        return ValidationResult.Success;
    }
}