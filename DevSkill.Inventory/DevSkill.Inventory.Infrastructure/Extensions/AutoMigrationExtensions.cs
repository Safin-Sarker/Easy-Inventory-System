using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Extensions
{
    public static class AutoMigrationExtensions
    {
        public static void AutoMigrations(this IServiceProvider services) 
        {
            using var scope = services.CreateScope();

            var inventorydbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

            if(inventorydbContext.Database.GetPendingMigrations().Any()) 
            {
                inventorydbContext.Database.Migrate();

            }

            var applicationdbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (applicationdbcontext.Database.GetPendingMigrations().Any())
            {
                applicationdbcontext.Database.Migrate();


            }

            SeedApplicationDbContext(applicationdbcontext);

        }
        private static void SeedApplicationDbContext(ApplicationDbContext context)
        {
            // Seed roles
            if (!context.Roles.Any())
            {
                var superAdminRoleId = Guid.Parse("b136d304-ed89-4084-8a3d-521ecda7bb7e");
                var memberRoleId = Guid.Parse("a2d7c304-ad89-4084-8a3d-421ecda7bb7f");

                context.Roles.AddRange(
                    new ApplicationRole
                    {
                        Id = superAdminRoleId,
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN"
                    },
                    new ApplicationRole
                    {
                        Id = memberRoleId,
                        Name = "Member",
                        NormalizedName = "MEMBER"
                    }
                );
            }

            // Seed users
            if (!context.Users.Any())
            {
                var superAdminUserId = Guid.Parse("dd5b6529-e2f1-4e98-ad4d-bd33432de0e0");

                context.Users.Add(new ApplicationUser
                {
                    Id = superAdminUserId,
                    UserName = "superadmin@example.com",
                    NormalizedUserName = "SUPERADMIN@EXAMPLE.COM",
                    Email = "superadmin@example.com",
                    NormalizedEmail = "SUPERADMIN@EXAMPLE.COM",
                    EmailConfirmed = true,
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "SuperAdmin@123"),
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Status = "Active"
                });

                // Assign SuperAdmin user to SuperAdmin role
                context.UserRoles.Add(new ApplicationUserRole
                {
                    UserId = superAdminUserId,
                    RoleId = Guid.Parse("b136d304-ed89-4084-8a3d-521ecda7bb7e")
                });
            }

            // Seed role claims
            if (!context.RoleClaims.Any())
            {
                var superAdminRoleId = Guid.Parse("b136d304-ed89-4084-8a3d-521ecda7bb7e");
                var memberRoleId = Guid.Parse("a2d7c304-ad89-4084-8a3d-421ecda7bb7f");

                context.RoleClaims.AddRange(
                    // SuperAdmin Claims
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
                    new ApplicationRoleClaim { Id = -32, RoleId = superAdminRoleId, ClaimType = "Permission", ClaimValue = "RoleDelete" },

                    // Member Claims
                    new ApplicationRoleClaim { Id = -101, RoleId = memberRoleId, ClaimType = "Permission", ClaimValue = "ItemViewDetails" },
                    new ApplicationRoleClaim { Id = -102, RoleId = memberRoleId, ClaimType = "Permission", ClaimValue = "ConsumptionView" },
                    new ApplicationRoleClaim { Id = -103, RoleId = memberRoleId, ClaimType = "Permission", ClaimValue = "ProductionGet" },
                    new ApplicationRoleClaim { Id = -104, RoleId = memberRoleId, ClaimType = "Permission", ClaimValue = "TransferView" },
                    new ApplicationRoleClaim { Id = -105, RoleId = memberRoleId, ClaimType = "Permission", ClaimValue = "WarehouseView" }
                );
            }

            context.SaveChanges();
        }
    }
}
