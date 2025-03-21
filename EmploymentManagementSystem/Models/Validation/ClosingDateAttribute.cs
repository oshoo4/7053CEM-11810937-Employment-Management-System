using System.ComponentModel.DataAnnotations;
using EmploymentManagementSystem.ViewModels;

namespace EmploymentManagementSystem.Models.Validation
{
    // INHERITANCE (Object-Oriented Programming)
    // HIGH COHESION(GRASP Patterns)
    public class ClosingDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value,
            ValidationContext validationContext
        )
        {
            if (validationContext.ObjectInstance is JobOpportunity jobOpportunity)
            {
                var closingDate = (DateOnly)value;

                if (closingDate <= jobOpportunity.PostedDate)
                {
                    return new ValidationResult("Closing Date must be after Posted Date.");
                }
            }
            else if (
                validationContext.ObjectInstance is CreateJobOpportunityViewModel createViewModel
            )
            {
                var closingDate = (DateOnly)value;
                if (closingDate <= createViewModel.PostedDate)
                {
                    return new ValidationResult("Closing Date must be after Posted Date.");
                }
            }
            else if (validationContext.ObjectInstance is EditJobOpportunityViewModel editViewModel)
            {
                var closingDate = (DateOnly)value;
                if (closingDate <= editViewModel.PostedDate)
                {
                    return new ValidationResult("Closing Date must be after Posted Date.");
                }
            }
            else
            {
                return new ValidationResult("Invalid object type for validation.");
            }

            return ValidationResult.Success;
        }
    }
}
