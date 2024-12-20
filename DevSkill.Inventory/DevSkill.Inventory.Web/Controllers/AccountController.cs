﻿


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Models;
using Microsoft.AspNetCore.Authorization;
using DevSkill.Inventory.Domain;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace DevSkill.Inventory.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailUtility _emailUtility;



        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IEmailUtility emailUtility
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailUtility = emailUtility;
        }



        [AllowAnonymous]
         public async Task<IActionResult> RegisterAsync(string returnUrl = null)
         {
            var model = new RegistrationModel();
            model.ReturnUrl = returnUrl;
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

             return View(model);
         }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegistrationModel model)
        {
            model.ReturnUrl ??= Url.Content("~/");
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "This email address is already taken.");
                    return View(model); // Return the view with the error message
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,Status = "Active" };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Action("ConfirmEmail",
                        "Account",

                        values: new { area = "", userId = user.Id, code = code, returnUrl = model.ReturnUrl },
                        protocol: Request.Scheme);

                    _emailUtility.SendEmail(model.Email, model.Email, "Confirm your mail",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");



                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("RegisterConfirmation", new { email = model.Email, returnUrl = model.ReturnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(model.ReturnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model); // Capitalize 'View'
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new SigninModel();

            // Check if "Remember Me" cookie exists
            if (Request.Cookies.TryGetValue("RememberMeFunc", out string rememberMeToken))
            {
                // Decode the token (e.g., email stored in the cookie)
                var tokenParts = rememberMeToken.Split('|');
                if (tokenParts.Length == 2)
                {
                  ViewBag.RememberMeEmail = tokenParts[0];
                  ViewBag.RememberMePassword = tokenParts[1];
                }
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> LoginAsync(SigninModel model)
        {
            model.ReturnUrl ??= Url.Action("Index", "Dashboard", new { Area = "Admin" });
            model.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }

                    if (user.Status == "Inactive")
                    {
                        ModelState.AddModelError(string.Empty, "Your account is inactive. Please contact support.");
                        return View(model);
                    }

                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        // Remember Me functionality
                        if (model.RememberMe)
                        {
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = true,
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTime.Now.AddDays(14)
                            };

                            Response.Cookies.Append("RememberMeFunc", $"{user.Email}|{model.Password}", cookieOptions);
                        }

                        _logger.LogInformation("User logged in successfully.");
                        return LocalRedirect(model.ReturnUrl);
                    }

                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToAction("LoginWith2fa", new { ReturnUrl = model.ReturnUrl, model.RememberMe });
                    }

                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToAction("Lockout");
                    }

                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during the login attempt.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                }
            }

            return View(model);
        }
        public async Task<IActionResult> LogoutAsync(string returnUrl = null)
        {

            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            returnUrl ??= Url.Content("~/");
            return LocalRedirect(returnUrl);

        }

        public IActionResult AccessDenied()
        {
            return View();
        }





    }

}
