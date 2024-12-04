using DevSkill.Inventory.Application.Services.RoleManagementService;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.RoleModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRoleManagementService _roleManagementService;
        private readonly ILogger<RoleController> _logger;

        public RoleController(RoleManager<ApplicationRole> roleManager,
            IRoleManagementService roleManagementService,
            ILogger<RoleController> logger)
        {
            _roleManager = roleManager;
            _roleManagementService = roleManagementService;
            _logger = logger;
        }

        [Authorize(Policy = "CanViewRoles")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> RoleJsonData([FromBody] RoleListModel model)
        {
            var result = await _roleManagementService.GetPagedRolesAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name") 
            );

            var itemJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = result.data.Select(role => new
                {
                    Id = role.Id.ToString(),
                    Name = role.Name
                }).ToList()
            };

            return Json(itemJsonData);
        }

        [HttpGet, Authorize(Policy = "CanCreateRole")]
        public IActionResult Create()
        {
            var model = new CreateRoleViewModel
            {
                Permissions = new List<PermissionViewModel>
                {
                    // Item Permissions
                    new PermissionViewModel { Name = "ItemCreate", DisplayName = "Can create item", Category = "Item", IsSelected = false },
                    new PermissionViewModel { Name = "ItemUpdate", DisplayName = "Can update item", Category = "Item", IsSelected = false },
                    new PermissionViewModel { Name = "ItemDelete", DisplayName = "Can delete item", Category = "Item", IsSelected = false },
                    new PermissionViewModel { Name = "ItemGet", DisplayName = "Can get item", Category = "Item", IsSelected = false },
                    new PermissionViewModel { Name = "ItemViewDetails", DisplayName = "Can view item details", Category = "Item", IsSelected = false },

                    // Record Consumption Permissions
                    new PermissionViewModel { Name = "ConsumptionCreate", DisplayName = "Can create Consumption", Category = "Record Consumption", IsSelected = false },
                    new PermissionViewModel { Name = "ConsumptionView", DisplayName = "Can view Consumption", Category = "Record Consumption", IsSelected = false },
                    new PermissionViewModel { Name = "ConsumptionDelete", DisplayName = "Can delete Consumption", Category = "Record Consumption", IsSelected = false },
                    new PermissionViewModel { Name = "ConsumptionGetDetails", DisplayName = "Can view Consumption details", Category = "Record Consumption", IsSelected = false },
                    new PermissionViewModel { Name = "ConsumptionGeneratePDF", DisplayName = "Can generate PDF of Consumption details", Category = "Record Consumption", IsSelected = false },

                    // Record Production Permissions

                    new PermissionViewModel { Name = "ProductionCreate", DisplayName = "Can create Production", Category = "Record Production", IsSelected = false },
                    new PermissionViewModel { Name = "ProductionGet", DisplayName = "Can View Production", Category = "Record Production", IsSelected = false },
                    new PermissionViewModel { Name = "ProductionDelete", DisplayName = "Can delete Production", Category = "Record Production", IsSelected = false },
                    new PermissionViewModel { Name = "ProductionGetDetails", DisplayName = "Can view Production details", Category = "Record Production", IsSelected = false },
                    new PermissionViewModel { Name = "ProductionGenerateReport", DisplayName = "Can generate report for Production", Category = "Record Production", IsSelected = false },

                    // Record Transfer Permissions
                    new PermissionViewModel { Name = "TransferCreate", DisplayName = "Can create Transfer", Category = "Record Transfer", IsSelected = false },
                    new PermissionViewModel { Name = "TransferView", DisplayName = "Can view Transfers", Category = "Record Transfer", IsSelected = false },
                    new PermissionViewModel { Name = "TransferDelete", DisplayName = "Can delete Transfers", Category = "Record Transfer", IsSelected = false },
                    new PermissionViewModel { Name = "TransferGetDetails", DisplayName = "Can view Transfer details", Category = "Record Transfer", IsSelected = false },
                    new PermissionViewModel { Name = "TransferGenerateReport", DisplayName = "Can generate Transfer report", Category = "Record Transfer", IsSelected = false },


                    // Warehouse Permissions
                    new PermissionViewModel { Name = "WarehouseCreate", DisplayName = "Can create Warehouse", Category = "Warehouse", IsSelected = false },
                    new PermissionViewModel { Name = "WarehouseView", DisplayName = "Can view Warehouses", Category = "Warehouse", IsSelected = false },
                    new PermissionViewModel { Name = "WarehouseUpdate", DisplayName = "Can update Warehouses", Category = "Warehouse", IsSelected = false },
                    new PermissionViewModel { Name = "WarehouseDelete", DisplayName = "Can delete Warehouses", Category = "Warehouse", IsSelected = false },

                     // Employee Management Permissions
                    new PermissionViewModel { Name = "UserCreate", DisplayName = "Can create User", Category = "Employee Management", IsSelected = false },
                    new PermissionViewModel { Name = "UserView", DisplayName = "Can view Users", Category = "Employee Management", IsSelected = false },
                    new PermissionViewModel { Name = "UserUpdate", DisplayName = "Can update User", Category = "Employee Management", IsSelected = false },
                    new PermissionViewModel { Name = "UserDelete", DisplayName = "Can delete User", Category = "Employee Management", IsSelected = false },

                    // Role Management Permissions
                    new PermissionViewModel { Name = "RoleCreate", DisplayName = "Can create Role", Category = "Role Management", IsSelected = false },
                    new PermissionViewModel { Name = "RoleView", DisplayName = "Can view Roles", Category = "Role Management", IsSelected = false },
                    new PermissionViewModel { Name = "RoleUpdate", DisplayName = "Can update Role", Category = "Role Management", IsSelected = false },
                    new PermissionViewModel { Name = "RoleDelete", DisplayName = "Can delete Role", Category = "Role Management", IsSelected = false }

                }
            };

            return View(model);
        }

        [HttpPost, Authorize(Policy = "CanCreateRole")]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to create role. ",
                        Type = ResponseType.Danger
                    });
                    return View(model); 
                }

                if (await _roleManager.RoleExistsAsync(model.RoleName))
                {
                    ModelState.AddModelError("RoleName", "Role already exists.");
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role creation failed. The role already exists.",
                        Type = ResponseType.Danger
                    });
                    return View(model);
                }

                // Create the new role
                var role = new ApplicationRole { Name = model.RoleName };
                var result = await _roleManager.CreateAsync(role);

                if (!result.Succeeded)
                {

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to create role",
                        Type = ResponseType.Danger
                    });
                    return View(model);
                }
                var selectedPermissions = model.Permissions
                    .Where(p => p.IsSelected)
                    .Select(p => p.Name)
                    .ToList();

                foreach (var permission in selectedPermissions)
                {
                    var claim = new Claim("Permission", permission);
                    await _roleManager.AddClaimAsync(role, claim);
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Role created successfully with the selected permissions.",
                    Type = ResponseType.success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                 _logger.LogError(ex, "An error occurred while creating the role.");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Role creation failed.",
                    Type = ResponseType.Danger
                });

                return View(model); 
            }
        }



        [HttpGet, Authorize(Policy = "CanUpdateRole")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var parsedId))
            {
                return BadRequest("Invalid role ID.");
            }


            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var claims = await _roleManager.GetClaimsAsync(role);

            var model = new UpdateRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = new List<PermissionViewModel>
                {
                    // Item Permissions
                    new PermissionViewModel
                    {
                        Name = "ItemCreate",
                        DisplayName = "Can create item",
                        Category = "Item",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ItemCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "ItemUpdate",
                        DisplayName = "Can update item",
                        Category = "Item",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ItemUpdate")
                    },
                    new PermissionViewModel
                    {
                        Name = "ItemDelete",
                        DisplayName = "Can delete item",
                        Category = "Item",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ItemDelete")
                    },
                    new PermissionViewModel
                    {
                        Name = "ItemGet",
                        DisplayName = "Can get item",
                        Category = "Item",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ItemGet")
                    },
                    new PermissionViewModel
                    {
                        Name = "ItemViewDetails",
                        DisplayName = "Can view item details",
                        Category = "Item",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ItemViewDetails")
                    },

                    // Record Consumption Permissions
                    new PermissionViewModel
                    {
                        Name = "ConsumptionCreate",
                        DisplayName = "Can create Consumption",
                        Category = "Record Consumption",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ConsumptionCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "ConsumptionView",
                        DisplayName = "Can view Consumption",
                        Category = "Record Consumption",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ConsumptionView")
                    },
                    new PermissionViewModel
                    {
                        Name = "ConsumptionDelete",
                        DisplayName = "Can delete Consumption",
                        Category = "Record Consumption",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ConsumptionDelete")
                    },
                    new PermissionViewModel
                    {
                        Name = "ConsumptionGetDetails",
                        DisplayName = "Can view Consumption details",
                        Category = "Record Consumption",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ConsumptionGetDetails")
                    },
                    new PermissionViewModel
                    {
                        Name = "ConsumptionGeneratePDF",
                        DisplayName = "Can generate PDF of Consumption details",
                        Category = "Record Consumption",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ConsumptionGeneratePDF")
                    },

                    // Record Production Permissions
                    new PermissionViewModel
                    {
                        Name = "ProductionCreate",
                        DisplayName = "Can create Production",
                        Category = "Record Production",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ProductionCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "ProductionGet",
                        DisplayName = "Can View Production",
                        Category = "Record Production",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ProductionUpdate")
                    },
                    new PermissionViewModel
                    {
                        Name = "ProductionDelete",
                        DisplayName = "Can delete Production",
                        Category = "Record Production",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ProductionDelete")
                    },
                    new PermissionViewModel
                    {
                        Name = "ProductionGetDetails",
                        DisplayName = "Can view Production details",
                        Category = "Record Production",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ProductionGetDetails")
                    },
                    new PermissionViewModel
                    {
                        Name = "ProductionGenerateReport",
                        DisplayName = "Can generate report for Production",
                        Category = "Record Production",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "ProductionGenerateReport")
                    },

                    // Record Transfer Permissions
                    new PermissionViewModel
                    {
                        Name = "TransferCreate",
                        DisplayName = "Can create Transfer",
                        Category = "Record Transfer",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "TransferCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "TransferView",
                        DisplayName = "Can view Transfers",
                        Category = "Record Transfer",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "TransferView")
                    },
                    new PermissionViewModel
                    {
                        Name = "TransferDelete",
                        DisplayName = "Can delete Transfers",
                        Category = "Record Transfer",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "TransferDelete")
                    },
                    new PermissionViewModel
                    {
                        Name = "TransferGetDetails",
                        DisplayName = "Can view Transfer details",
                        Category = "Record Transfer",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "TransferGetDetails")
                    },
                    new PermissionViewModel
                    {
                        Name = "TransferGenerateReport",
                        DisplayName = "Can generate Transfer report",
                        Category = "Record Transfer",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "TransferGenerateReport")
                    },


                    // Warehouse Permissions
                    new PermissionViewModel
                    {
                        Name = "WarehouseCreate",
                        DisplayName = "Can create Warehouse",
                        Category = "Warehouse",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "WarehouseCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "WarehouseView",
                        DisplayName = "Can view Warehouses",
                        Category = "Warehouse",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "WarehouseView")
                    },
                    new PermissionViewModel
                    {
                        Name = "WarehouseUpdate",
                        DisplayName = "Can update Warehouses",
                        Category = "Warehouse",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "WarehouseUpdate")
                    },
                    new PermissionViewModel
                    {
                        Name = "WarehouseDelete",
                        DisplayName = "Can delete Warehouses",
                        Category = "Warehouse",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "WarehouseDelete")
                    },

                    // Employee Management Permissions
                    new PermissionViewModel
                    {
                        Name = "UserCreate",
                        DisplayName = "Can create User",
                        Category = "Employee Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "UserCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "UserView",
                        DisplayName = "Can view Users",
                        Category = "Employee Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "UserView")
                    },
                    new PermissionViewModel
                    {
                        Name = "UserUpdate",
                        DisplayName = "Can update User",
                        Category = "Employee Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "UserUpdate")
                    },
                    new PermissionViewModel
                    {
                        Name = "UserDelete",
                        DisplayName = "Can delete User",
                        Category = "Employee Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "UserDelete")
                    },

                    // Role Management Permissions
                    new PermissionViewModel
                    {
                        Name = "RoleCreate",
                        DisplayName = "Can create Role",
                        Category = "Role Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "RoleCreate")
                    },
                    new PermissionViewModel
                    {
                        Name = "RoleView",
                        DisplayName = "Can view Roles",
                        Category = "Role Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "RoleView")
                    },
                    new PermissionViewModel
                    {
                        Name = "RoleUpdate",
                        DisplayName = "Can update Role",
                        Category = "Role Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "RoleUpdate")
                    },
                    new PermissionViewModel
                    {
                        Name = "RoleDelete",
                        DisplayName = "Can delete Role",
                        Category = "Role Management",
                        IsSelected = claims.Any(c => c.Type == "Permission" && c.Value == "RoleDelete")
                    }
                }
            };

            return View(model);
        }

        [HttpPost, Authorize(Policy = "CanUpdateRole")]
        public async Task<IActionResult> Edit(UpdateRoleViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update role. Please correct the errors and try again.",
                        Type = ResponseType.Danger
                    });
                    return View(model); 
                }

               
                var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                if (role == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role not found.",
                        Type = ResponseType.Danger
                    });
                    return NotFound();
                }

                // Update role name
                role.Name = model.RoleName;
                var updateResult = await _roleManager.UpdateAsync(role);

                if (!updateResult.Succeeded)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Failed to update role.",
                        Type = ResponseType.Danger
                    });
                    return View(model); 
                }

               
                var existingClaims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in existingClaims.Where(c => c.Type == "Permission"))
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }

                var selectedPermissions = model.Permissions
                    .Where(p => p.IsSelected)
                    .Select(p => new Claim("Permission", p.Name));

                foreach (var claim in selectedPermissions)
                {
                    await _roleManager.AddClaimAsync(role, claim);
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Role updated successfully with the selected permissions.",
                    Type = ResponseType.success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
 
                 _logger.LogError(ex, "An error occurred while updating the role.");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An unexpected error occurred while updating the role. Please try again later.",
                    Type = ResponseType.Danger
                });

                return View(model); 
            }
        }


        [HttpPost, Authorize(Policy = "CanDeleteRole")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Invalid role ID.",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }

                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Role not found.",
                        Type = ResponseType.Danger
                    });
                    return RedirectToAction("Index");
                }

               
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    var removeClaimResult = await _roleManager.RemoveClaimAsync(role, claim);
                    if (!removeClaimResult.Succeeded)
                    {
                        TempData["ResponseMessage"] = new ResponseModel
                        {
                            Message = "Failed to remove associated claims from the role.",
                            Type = ResponseType.Danger
                        };
                        return RedirectToAction("Index");
                    }
                }

               
                var deleteResult = await _roleManager.DeleteAsync(role);
                if (!deleteResult.Succeeded)
                {
                    foreach (var error in deleteResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    TempData["ResponseMessage"] = new ResponseModel
                    {
                        Message = "Role deletion Failed",
                        Type = ResponseType.Danger
                    };
                    return RedirectToAction("Index");
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Role deleted successfully.",
                    Type = ResponseType.success
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the role.");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Role deletion Failed",
                    Type = ResponseType.Danger
                });

                return RedirectToAction("Index");
            }
        }







    }
}
