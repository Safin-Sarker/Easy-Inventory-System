using Microsoft.AspNetCore.Identity;
using System;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    }
}
