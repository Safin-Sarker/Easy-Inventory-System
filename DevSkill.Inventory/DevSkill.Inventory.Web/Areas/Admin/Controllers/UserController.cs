using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models.User_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;
using System.Linq.Dynamic.Core;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevSkill.Inventory.Domain;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using AutoMapper;
using DevSkill.Inventory.Application.Services.UserManagement;


namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly IEmailUtility _emailUtility;
        private readonly IUserManagementService _userManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManager,
           ApplicationRoleManager roleManager,
           IEmailUtility emailUtility,
           IUserManagementService userManagementService,
           IMapper mapper,
           ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailUtility = emailUtility;
            _userManagementService = userManagementService;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "CanViewUsers")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> UserJsonData([FromBody] UserListModel model)
        {
            var result = await _userManagementService.GetPagedUsersAsync(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("UserName") 
            );

            var itemJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totalDisplay,
                data = result.data.Select(user => new
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Status = user.Status,
                    Roles = user.Roles  
                }).ToList()
            };

            return Json(itemJsonData);
        }


        [Authorize(Policy = "CanCreateUser")]
        public IActionResult AddUser()
        {
            var model = new AddUserModel
            {
                RoleList = GetRoleSelectList()
            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken,Authorize(Policy = "CanCreateUser")]
        public async Task<IActionResult> AddUser(AddUserModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email address is already taken.");
                    model.RoleList = GetRoleSelectList();
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User Creation Failed: Email already in use.",
                        Type = ResponseType.Danger
                    });
                    return View(model);
                }
                var user = _mapper.Map<ApplicationUser>(model);

                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        if (model.Roles != null && model.Roles.Any())
                        {
                            await _userManager.AddToRolesAsync(user, model.Roles);

                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                            var callbackUrl = Url.Action("ConfirmEmail",
                                "Account",
                                values: new { area = "", userId = user.Id, code = code },
                                protocol: Request.Scheme);

                            _emailUtility.SendEmail(model.Email, model.Email, "Confirm your mail",
                                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                        }

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "User Created Successfully",
                            Type = ResponseType.success
                        });

                        return RedirectToAction("Index", "User", new { area = "Admin" });
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                            _logger.LogError("User creation error: {Error}", error.Description);
                        }

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "User Creation Failed: " + string.Join(", ", result.Errors.Select(e => e.Description)),
                            Type = ResponseType.Danger
                        });
                    }
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "An error occurred while creating the user",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "An error occurred while creating the user");
                }
            }
            else
            {
                model.RoleList = GetRoleSelectList();

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Please correct the errors and try again.",
                    Type = ResponseType.Danger
                });
            }

            model.RoleList = model.RoleList ?? GetRoleSelectList();
            return View(model);
        }


        [HttpGet, Authorize(Policy = "CanUpdateUser")]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var model = _mapper.Map<EditUserModel>(user);

                model.Roles = await _userManager.GetRolesAsync(user);
                model.RoleList = GetRoleSelectList(model.Roles);

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to edit the user with ID {UserId}", id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An error occurred while loading the user data.",
                    Type = ResponseType.Danger
                });

                return RedirectToAction("Index");
            }
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanUpdateUser")]
        public async Task<IActionResult> Edit(EditUserModel model)
        {
            if (!model.EnablePasswordReset)
            {
                ModelState.Remove(nameof(model.CurrentPassword));
                ModelState.Remove(nameof(model.NewPassword));
                ModelState.Remove(nameof(model.ConfirmPassword));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(model.Id.ToString());
                    if (user == null)
                    {
                        return NotFound();
                    }

                    _mapper.Map(model, user);

                    var updateResult = await _userManager.UpdateAsync(user);
                    if (updateResult.Succeeded)
                    {
                        if (!string.IsNullOrWhiteSpace(model.NewPassword))
                        {
                            if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                            {
                                ModelState.AddModelError("CurrentPassword", "Current password is required to set a new password.");
                                model.RoleList = GetRoleSelectList(model.Roles);
                                return View(model);
                            }

                            var passwordCheck = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                            if (!passwordCheck)
                            {
                                ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                                model.RoleList = GetRoleSelectList(model.Roles);
                                return View(model);
                            }

                            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                            var passwordChangeResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
                            if (!passwordChangeResult.Succeeded)
                            {
                                foreach (var error in passwordChangeResult.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                                model.RoleList = GetRoleSelectList(model.Roles);
                                return View(model);
                            }
                        }
                        var userRoles = await _userManager.GetRolesAsync(user);
                        var rolesToAdd = model.Roles.Except(userRoles);
                        var rolesToRemove = userRoles.Except(model.Roles);

                        if (rolesToAdd.Any())
                        {
                            await _userManager.AddToRolesAsync(user, rolesToAdd);
                        }
                        if (rolesToRemove.Any())
                        {
                            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                        }

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "User updated successfully",
                            Type = ResponseType.success
                        });

                        return RedirectToAction("Index", "User", new { area = "Admin" });
                    }
                    else
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "User update failed",
                            Type = ResponseType.Danger
                        });
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the user with ID {UserId}", model.Id);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "An error occurred while updating the user.",
                        Type = ResponseType.Danger
                    });
                }
            }
            else
            {
                model.RoleList = GetRoleSelectList(model.Roles);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Please correct the errors and try again.",
                    Type = ResponseType.Danger
                });
            }

            model.RoleList = model.RoleList ?? GetRoleSelectList(model.Roles);
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, Authorize(Policy = "CanDeleteUser")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Any())
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!removeRolesResult.Succeeded)
                    {
                     

                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Failed to remove user from roles.",
                            Type = ResponseType.Danger
                        });

                        return RedirectToAction("Index"); 
                    }
                }

                var deleteResult = await _userManager.DeleteAsync(user);
                if (deleteResult.Succeeded)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "User deleted successfully.",
                        Type = ResponseType.success
                    });

                    return RedirectToAction("Index");
                }

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "User deletion failed.",
                    Type = ResponseType.Danger
                });

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the user with ID {UserId}", id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "An unexpected error occurred while trying to delete the user.",
                    Type = ResponseType.Danger
                });

                return RedirectToAction("Index"); 
            }
        }



        private IEnumerable<SelectListItem> GetRoleSelectList(IEnumerable<string> selectedRoles = null)
        {
            return _roleManager.Roles.Select(role => new SelectListItem
            {
                Value = role.Name,
                Text = role.Name,
                Selected = selectedRoles != null && selectedRoles.Contains(role.Name)
            }).ToList();
        }








    }
}



