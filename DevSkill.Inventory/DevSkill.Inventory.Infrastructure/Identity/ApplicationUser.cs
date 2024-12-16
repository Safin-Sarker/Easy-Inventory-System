using Microsoft.AspNetCore.Identity;
using System;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Status { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? BloodGroup { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Locale { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? TimeZone { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public ICollection<ApplicationUserRole>? UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}
