using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models.User_Models
{
    public class AddUserModel
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "First Name should not contain spaces or special characters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "Last Name should not contain spaces or special characters.")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$", ErrorMessage = "User Name should not contain spaces or special characters.")]
        public string UserName { get; set; }

        public string? ContactNo { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Status { get; set; }
        public string[] Roles { get; set; }
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}

