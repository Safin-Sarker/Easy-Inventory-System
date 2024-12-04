using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.User_Models
{
    public class EditUserModel : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNo { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Roles")]
        public IList<string> Roles { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public bool EnablePasswordReset { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        public EditUserModel()
        {
            Roles = new List<string>();
            RoleList = new List<SelectListItem>();
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EnablePasswordReset)
            {
                if (string.IsNullOrEmpty(CurrentPassword))
                    yield return new ValidationResult("Current password is required.", new[] { nameof(CurrentPassword) });

                if (string.IsNullOrEmpty(NewPassword))
                    yield return new ValidationResult("New password is required.", new[] { nameof(NewPassword) });

                if (string.IsNullOrEmpty(ConfirmPassword))
                    yield return new ValidationResult("Confirm password is required.", new[] { nameof(ConfirmPassword) });
            }
        }
    }
}
