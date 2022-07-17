using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie247.Areas.Identity.Data;
using Movie247.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Movie247.Controllers.Common
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Movie247User> _signInManager;
        private readonly UserManager<Movie247User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<Movie247User> signInManager, UserManager<Movie247User> userManager, IEmailSender emailSender, ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public LoginModel InputLogin { get; set; }

        [BindProperty]
        public RegisterModel InputRegister { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public string ReturnUrl { get; set; }
        /** -------------------------------- Login -------------------------------- */
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ??= Url.Content("~/");
            if (!_signInManager.IsSignedIn(User))
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                ViewBag.ReturnUrl = ReturnUrl;
                return View("Login");
            }
            else
            {
                return LocalRedirect(returnUrl);
            }
        }
        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ??= Url.Content("~/");
            ViewBag.ReturnUrl = ReturnUrl;
            var user = await _userManager.FindByEmailAsync(InputLogin.Email);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(InputLogin.Email);
            }
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Account not found");
                return View("Login");
            }
            var result = await _signInManager.PasswordSignInAsync(user, InputLogin.Password, InputLogin.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                ModelState.AddModelError(string.Empty, "Account is locked out");
                return View("Login");
            }
            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User account is not allowed.");
                ModelState.AddModelError(string.Empty, "Account is not allowed, We sent you an email to confirm your account.");
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code, returnUrl = returnUrl }, protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                return View("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            }
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        /** -------------------------------- Register -------------------------------- */
        [HttpGet]
        public IActionResult RegisterAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ??= Url.Content("~/");
            ViewBag.ReturnUrl = ReturnUrl;
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (!string.IsNullOrEmpty(ErrorMessage))
                {
                    ModelState.AddModelError(string.Empty, ErrorMessage);
                }
                ReturnUrl = returnUrl;
                return View("Register");
            }
        }
        [HttpPost, ActionName("Register")]
        public async Task<IActionResult> RegisterPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ??= Url.Content("~/");
            ViewBag.ReturnUrl = ReturnUrl;
            try
            {
                var user = new Movie247User { UserName = InputRegister.UserName, Email = InputRegister.Email, Firstname = InputRegister.FirstName, LastName = InputRegister.LastName };
                var EmailExists = await _userManager.FindByEmailAsync(InputRegister.Email);
                var UserNameExists = await _userManager.FindByNameAsync(InputRegister.UserName);
                if (EmailExists != null || UserNameExists != null)
                {
                    if (EmailExists != null)
                    {
                        ModelState.AddModelError(string.Empty, "Email already exists");
                    }
                    if (UserNameExists != null)
                    {
                        ModelState.AddModelError(string.Empty, "Username already exists");
                    }
                    return View("Register");
                }
                var result = await _userManager.CreateAsync(user, InputRegister.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code, returnUrl = returnUrl }, protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(InputRegister.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToAction("Index", "Redirect", new { message = "Please check your email and confirm your account, you must be confirmed before you can log in" });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }

            // If we got this far, something failed, redisplay form
            return View("Register");
        }

        /** -------------------------------- ConfirmEmail -------------------------------- */
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return RedirectToAction("Index", "Redirect", new { message = result.Succeeded ? "Thank you for confirming your email." : "Invalid confirmation code or expired link." });
        }

        /** -------------------------------- ForgotPassword -------------------------------- */
        public async Task<IActionResult> ForgotPassword()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                ViewData["Email"] = user.Email;
            }
            ViewData["Message"] = "Enter your email address and we'll send you a link to reset your password.";
            return View("ForgotPassword");
        }
        [HttpPost, ActionName("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(InputLogin.Email);
            if (user == null)
            {
                ViewData["Message"] = "Email not found";
                return View("ForgotPassword");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(InputLogin.Email, "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            return RedirectToAction("Index", "Redirect", new { message = "Please check your email to reset your password." });
        }

        /** -------------------------------- ResetPassword -------------------------------- */
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                ViewData["code"] = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                return View("ResetPassword");
            }
        }

        [HttpPost, ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordPostAsync()
        {
            var user = await _userManager.FindByEmailAsync(InputRegister.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Index", "Redirect", new { message = "Email reset not found" });
            }

            var result = await _userManager.ResetPasswordAsync(user, InputRegister.Code, InputRegister.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Redirect", new { message = "Your password has been reset." });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View("ResetPassword");
        }

        /** -------------------------------- Logout -------------------------------- */
        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/");
            }
        }

        public class RegisterModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            public string Code { get; set; }
        }
        public class LoginModel
        {
            [Required(ErrorMessage = "Email or Username is required")]
            [Display(Name = "Email or Username")]
            public string Email { get; set; }


            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Password is required")]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}
