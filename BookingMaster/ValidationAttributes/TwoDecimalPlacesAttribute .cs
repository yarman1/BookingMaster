using System.ComponentModel.DataAnnotations;

namespace BookingMaster.ValidationAttributes
{
    public class TwoDecimalPlacesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is decimal decimalValue)
            {
                if (decimal.Round(decimalValue, 2) != decimalValue)
                {
                    return new ValidationResult("Value cannot have more than 2 decimal places.");
                }
            }
            return ValidationResult.Success!;
        }
    }
}
