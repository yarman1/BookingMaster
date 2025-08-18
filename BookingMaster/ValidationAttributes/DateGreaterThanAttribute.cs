using System.ComponentModel.DataAnnotations;

namespace BookingMaster.ValidationAttributes
{
    public class DateGreaterThanAttribute(string comparisonProperty) : ValidationAttribute
    {
        private readonly string _comparisonProperty = comparisonProperty;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty) ?? throw new ArgumentException("Property with this name not found");
            var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue.HasValue && comparisonValue.HasValue &&
                currentValue.Value <= comparisonValue.Value)
            {
                return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be after {_comparisonProperty}");
            }

            return ValidationResult.Success;
        }
    
    }
}
