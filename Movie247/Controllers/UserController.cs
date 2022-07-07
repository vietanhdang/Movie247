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

namespace Movie247.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<Movie247User> _userManager;
        private readonly SignInManager<Movie247User> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;

        public UserController(SignInManager<Movie247User> signInManager,
            ILogger<LoginModel> logger,
            UserManager<Movie247User> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }


        [HttpPost]
        public async Task<JsonResult> LoginAsync([Bind("Email, Password, RememberMe")] LoginModel Input)
        {

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                // login username or email
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null)
                {
                    user = await _userManager.FindByNameAsync(Input.Email);
                }
                if (user == null)
                {
                    return Json(new { success = false, message = "Account does not exist" });
                }
                // var result = await _signInManager.CheckPasswordSignInAsync(user, Input.Password, Input.RememberMe);
                var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return new JsonResult(new { success = true, message = "Login Successful" });
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new JsonResult(new { success = false, message = $"Login failed. Account is locked out for {_userManager.Options.Lockout.DefaultLockoutTimeSpan.TotalMinutes} minutes" });
                }
                else
                {
                    return new JsonResult(new { success = false, message = "Wrong password" });
                }
            }
            else
            {
                return new JsonResult(new { success = false, message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList() });
            }
        }
        public async Task<JsonResult> RegisterAsync(string returnUrl = null, [Bind("Email,Password,ConfirmPassword,FirstName,LastName,UserName")] RegisterModel RegisterInput = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Movie247User { UserName = RegisterInput.UserName, Email = RegisterInput.Email, Firstname = RegisterInput.FirstName, LastName = RegisterInput.LastName };
                // check if email or username already exists
                var EmailExists = await _userManager.FindByEmailAsync(RegisterInput.Email);
                var UserNameExists = await _userManager.FindByNameAsync(RegisterInput.UserName);
                if (EmailExists != null || UserNameExists != null)
                {
                    return new JsonResult(new { success = false, message = "Username or email already exists" });
                }
                var result = await _userManager.CreateAsync(user, RegisterInput.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, code = code }, protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(RegisterInput.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return new JsonResult(new { success = true, message = "Please check your email and confirm your account." });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return new JsonResult(new { success = true, message = "Registration Successful" });
                    }
                }
                else
                {
                    return new JsonResult(new { success = false, message = result.Errors.Select(x => x.Description).ToList() });
                }
            }
            else
            {
                return new JsonResult(new { success = false, message = ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage).ToList() });
            }
        }
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Redirect", new { redirectUrl = Url.Content("~/"), message = "Invalid parameters" });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Redirect", new { redirectUrl = Url.Content("~/"), message = $"Unable to load user with ID '{userId}'." });
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return RedirectToAction("Index", "Redirect", new { redirectUrl = Url.Content("~/"), message = result.Succeeded ? "Email Confirmed" : "Error Confirming Email" });
        }
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/");
            }
        }

    }

}
