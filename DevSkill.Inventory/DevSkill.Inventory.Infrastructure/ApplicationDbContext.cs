using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,
       ApplicationRole, Guid,
       ApplicationUserClaim, ApplicationUserRole,
       ApplicationUserLogin, ApplicationRoleClaim,
       ApplicationUserToken>

    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public ApplicationDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x => x.MigrationsAssembly(_migrationAssembly));
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });


            builder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationship between ApplicationUserRole and ApplicationRole
            builder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define GUIDs for consistent seeding
            var superAdminRoleId = Guid.Parse("b136d304-ed89-4084-8a3d-521ecda7bb7e");
            var superAdminUserId = Guid.Parse("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0");

            // Seed SuperAdmin Role
            var superAdminRole = new ApplicationRole
            {
                Id = superAdminRoleId,
                Name = "SuperAdmin",
                NormalizedName = "SUPERADMIN"
            };

            // Seed SuperAdmin User
            var superAdminUser = new ApplicationUser
            {
                Id = superAdminUserId,
                UserName = "superadmin@example.com",
                NormalizedUserName = "SUPERADMIN@EXAMPLE.COM",
                Email = "superadmin@example.com",
                NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "SuperAdmin@123"),
                Status = "Active",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Seed Role Claims
            var roleClaims = new List<ApplicationRoleClaim>
            {
                new ApplicationRoleClaim { Id = -1, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ItemCreate" },
                new ApplicationRoleClaim { Id = -2, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ItemUpdate" },
                new ApplicationRoleClaim { Id = -3, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ItemDelete" },
                new ApplicationRoleClaim { Id = -4, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ItemGet" },
                new ApplicationRoleClaim { Id = -5, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ItemViewDetails" },

                // Record Consumption Permissions
                new ApplicationRoleClaim { Id = -6, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ConsumptionCreate" },
                new ApplicationRoleClaim { Id = -7, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ConsumptionView" },
                new ApplicationRoleClaim { Id = -8, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ConsumptionDelete" },
                new ApplicationRoleClaim { Id = -9, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ConsumptionGetDetails" },
                new ApplicationRoleClaim { Id = -10, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ConsumptionGeneratePDF" },

                // Record Production Permissions
                new ApplicationRoleClaim { Id = -11, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ProductionCreate" },
                new ApplicationRoleClaim { Id = -12, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ProductionGet" },
                new ApplicationRoleClaim { Id = -13, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ProductionDelete" },
                new ApplicationRoleClaim { Id = -14, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ProductionGetDetails" },
                new ApplicationRoleClaim { Id = -15, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "ProductionGenerateReport" },

                // Record Transfer Permissions
                new ApplicationRoleClaim { Id = -16, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "TransferCreate" },
                new ApplicationRoleClaim { Id = -17, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "TransferView" },
                new ApplicationRoleClaim { Id = -18, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "TransferDelete" },
                new ApplicationRoleClaim { Id = -19, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "TransferGetDetails" },
                new ApplicationRoleClaim { Id = -20, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "TransferGenerateReport" },

                // Warehouse Permissions
                new ApplicationRoleClaim { Id = -21, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "WarehouseCreate" },
                new ApplicationRoleClaim { Id = -22, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "WarehouseView" },
                new ApplicationRoleClaim { Id = -23, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "WarehouseUpdate" },
                new ApplicationRoleClaim { Id = -24, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "WarehouseDelete" },

                // Employee Management Permissions
                new ApplicationRoleClaim { Id = -25, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "UserCreate" },
                new ApplicationRoleClaim { Id = -26, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "UserView" },
                new ApplicationRoleClaim { Id = -27, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "UserUpdate" },
                new ApplicationRoleClaim { Id = -28, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "UserDelete" },

                // Role Management Permissions
                new ApplicationRoleClaim { Id = -29, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "RoleCreate" },
                new ApplicationRoleClaim { Id = -30, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "RoleView" },
                new ApplicationRoleClaim { Id = -31, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "RoleUpdate" },
                new ApplicationRoleClaim { Id = -32, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "RoleDelete" }
            };
            // Seed User to Role
            var userRole = new ApplicationUserRole
            {
                UserId = superAdminUserId,
                RoleId = superAdminRoleId
            };

            // Add data to model builder
            builder.Entity<ApplicationRole>().HasData(superAdminRole);
            builder.Entity<ApplicationUser>().HasData(superAdminUser);
            builder.Entity<ApplicationUserRole>().HasData(userRole);
            builder.Entity<ApplicationRoleClaim>().HasData(roleClaims.ToArray());

        }

       

    }
}
