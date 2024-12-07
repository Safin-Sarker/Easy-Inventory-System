using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {

            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = false;
            });

            services.AddAuthorization(options =>
            {
                // Item Permissions
                options.AddPolicy("CanCreateItem", policy => policy.RequireClaim("Permission", "ItemCreate"));
                options.AddPolicy("CanUpdateItem", policy => policy.RequireClaim("Permission", "ItemUpdate"));
                options.AddPolicy("CanDeleteItem", policy => policy.RequireClaim("Permission", "ItemDelete"));
                options.AddPolicy("CanGetItem", policy => policy.RequireClaim("Permission", "ItemGet"));
                options.AddPolicy("CanViewItemDetails", policy => policy.RequireClaim("Permission", "ItemViewDetails"));

                // Record Consumption Permissions
                options.AddPolicy("CanCreateConsumption", policy => policy.RequireClaim("Permission", "ConsumptionCreate"));
                options.AddPolicy("CanViewConsumption", policy => policy.RequireClaim("Permission", "ConsumptionView"));
                options.AddPolicy("CanDeleteConsumption", policy => policy.RequireClaim("Permission", "ConsumptionDelete"));
                options.AddPolicy("CanGetConsumptionDetails", policy => policy.RequireClaim("Permission", "ConsumptionGetDetails"));
                options.AddPolicy("CanGenerateConsumptionPDF", policy => policy.RequireClaim("Permission", "ConsumptionGeneratePDF"));

                // Record Production Permissions
                options.AddPolicy("CanCreateProduction", policy => policy.RequireClaim("Permission", "ProductionCreate"));
                options.AddPolicy("CanViewProduction", policy => policy.RequireClaim("Permission", "ProductionGet"));
                options.AddPolicy("CanDeleteProduction", policy => policy.RequireClaim("Permission", "ProductionDelete"));
                options.AddPolicy("CanGetProductionDetails", policy => policy.RequireClaim("Permission", "ProductionGetDetails"));
                options.AddPolicy("CanGenerateProductionReport", policy => policy.RequireClaim("Permission", "ProductionGenerateReport"));

                //Record Transfer
                options.AddPolicy("CanCreateTransfer", policy => policy.RequireClaim("Permission", "TransferCreate"));
                options.AddPolicy("CanViewTransfer", policy => policy.RequireClaim("Permission", "TransferView"));
                options.AddPolicy("CanDeleteTransfer", policy => policy.RequireClaim("Permission", "TransferDelete"));
                options.AddPolicy("CanGetTransferDetails", policy => policy.RequireClaim("Permission", "TransferGetDetails"));
                options.AddPolicy("CanGenerateTransferReport", policy => policy.RequireClaim("Permission", "TransferGenerateReport"));

                // Warehouse Permissions
                options.AddPolicy("CanCreateWarehouse", policy => policy.RequireClaim("Permission", "WarehouseCreate"));
                options.AddPolicy("CanViewWarehouses", policy => policy.RequireClaim("Permission", "WarehouseView"));
                options.AddPolicy("CanUpdateWarehouse", policy => policy.RequireClaim("Permission", "WarehouseUpdate"));
                options.AddPolicy("CanDeleteWarehouse", policy => policy.RequireClaim("Permission", "WarehouseDelete"));

                // Employee Management Permissions
                options.AddPolicy("CanCreateUser", policy => policy.RequireClaim("Permission", "UserCreate"));
                options.AddPolicy("CanViewUsers", policy => policy.RequireClaim("Permission", "UserView"));
                options.AddPolicy("CanUpdateUser", policy => policy.RequireClaim("Permission", "UserUpdate"));
                options.AddPolicy("CanDeleteUser", policy => policy.RequireClaim("Permission", "UserDelete"));

                // Role Management Permissions
                options.AddPolicy("CanCreateRole", policy => policy.RequireClaim("Permission", "RoleCreate"));
                options.AddPolicy("CanViewRoles", policy => policy.RequireClaim("Permission", "RoleView"));
                options.AddPolicy("CanUpdateRole", policy => policy.RequireClaim("Permission", "RoleUpdate"));
                options.AddPolicy("CanDeleteRole", policy => policy.RequireClaim("Permission", "RoleDelete"));
            });










        }
    }
}
