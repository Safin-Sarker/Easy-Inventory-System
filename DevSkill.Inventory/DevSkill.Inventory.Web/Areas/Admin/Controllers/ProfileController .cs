using AutoMapper;
using CloudinaryDotNet.Actions;
using DevSkill.Inventory.Application.Services.ImageManagement_Service;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using DevSkill.Inventory.Web.Areas.Admin.Models.User_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileController> _logger;
        private readonly IImageManagementService _imageManagementService;

        public ProfileController(UserManager<ApplicationUser> userManager,IMapper mapper,ILogger<ProfileController>logger,IImageManagementService imageManagementService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _imageManagementService = imageManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            var model = new UserProfileModel
            {
                Id=user.Id,
                FullName = user.FullName,
                Username = user.UserName,
                Email = user.Email,
                ContactNumber = user.PhoneNumber,
                Role = role ?? "No Role Assigned",
                Gender = user.Gender,
                BirthDate = user.BirthDate?.ToString("yyyy-MM-dd"),
                PresentAddress = user.PresentAddress,
                PermanentAddress = user.PermanentAddress,
                TimeZone = user.TimeZone,
                ProfilePictureUrl = user.ProfilePictureUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserProfileModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch existing user profile using UserManager's FindByIdAsync method
                    var existingUser = await _userManager.FindByIdAsync(model.Id.ToString());

                    if (existingUser == null)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "User profile not found",
                            Type = ResponseType.Danger
                        });
                        return RedirectToAction("Index");
                    }

                    // Manually map updated fields from model to the existing user
                    existingUser.FullName = model.FullName;
                    existingUser.PhoneNumber = model.ContactNumber;
                    existingUser.Gender = model.Gender;
                    existingUser.BirthDate = !string.IsNullOrEmpty(model.BirthDate)
                        ? DateTime.Parse(model.BirthDate)
                        : (DateTime?)null;
                    existingUser.PresentAddress = model.PresentAddress;
                    existingUser.PermanentAddress = model.PermanentAddress;
                    existingUser.TimeZone = model.TimeZone;

                    // Handle profile picture update using Cloudinary
                    if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
                    {
                        var imageUrl = await _imageManagementService.UploadAsync(model.ProfilePicture);
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            existingUser.ProfilePictureUrl = imageUrl; // Update the profile picture URL
                        }
                    }

                    // Update the user profile in the database using UserManager's UpdateAsync method
                    var updateResult = await _userManager.UpdateAsync(existingUser);

                    if (updateResult.Succeeded)
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Profile updated successfully",
                            Type = ResponseType.success
                        });

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData.Put("ResponseMessage", new ResponseModel
                        {
                            Message = "Profile update failed ",
                            Type = ResponseType.Danger
                        });
                    }
                }
                catch (Exception ex)
                {
                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Profile update failed",
                        Type = ResponseType.Danger
                    });

                    _logger.LogError(ex, "Profile update failed");
                }
            }

            return RedirectToAction("Index");
        }


    }
}
