using System.ComponentModel.DataAnnotations;
using EmploymentManagementSystem.Models.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmploymentManagementSystem.ViewModels
{
    public class EditJobOpportunityViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(100, ErrorMessage = "Location cannot be longer than 100 characters.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a non-negative value.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Posted Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateOnly PostedDate { get; set; }

        [Required(ErrorMessage = "Closing Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ClosingDate]
        public DateOnly ClosingDate { get; set; }

        public bool IsActive { get; set; }

        public List<SelectListItem> Companies { get; set; } = new();
    }
}
