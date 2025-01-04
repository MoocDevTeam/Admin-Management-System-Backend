using System.ComponentModel.DataAnnotations;

namespace Mooc.Core.CustomValidation
{
    /// <summary>
    /// Custom validation attribute to ensure StartDate is earlier than EndDate.
    /// </summary>
    public class DateTimeValidationAttribute : ValidationAttribute
    {
        private readonly string _startTimePropertyName;
        private readonly string _endTimePropertyName;

        public DateTimeValidationAttribute(string startTimePropertyName, string endTimePropertyName)
        {
            _startTimePropertyName = startTimePropertyName;
            _endTimePropertyName = endTimePropertyName;
        }

        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext
        )
        {
            // Get the type of the object being validated
            var objectType = validationContext.ObjectType;

            // Use reflection to get the StartTime and EndTime properties
            var startTimeProperty = objectType.GetProperty(_startTimePropertyName);
            var endTimeProperty = objectType.GetProperty(_endTimePropertyName);

            if (startTimeProperty == null || endTimeProperty == null)
            {
                return new ValidationResult(
                    $"Properties '{_startTimePropertyName}' or '{_endTimePropertyName}' not found."
                );
            }

            // Get the values of StartTime and EndTime
            var startTimeValue =
                startTimeProperty.GetValue(validationContext.ObjectInstance) as DateTime?;
            var endTimeValue =
                endTimeProperty.GetValue(validationContext.ObjectInstance) as DateTime?;

            if (startTimeValue == null || endTimeValue == null)
            {
                return new ValidationResult(
                    $"'{_startTimePropertyName}' and '{_endTimePropertyName}' must have valid DateTime values."
                );
            }

            // Validate that StartTime is earlier than EndTime
            if (startTimeValue >= endTimeValue)
            {
                return new ValidationResult(
                    $"'{_startTimePropertyName}' must be earlier than '{_endTimePropertyName}'."
                );
            }

            return ValidationResult.Success;
        }
    }
}
