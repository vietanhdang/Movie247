using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Movie247.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Movie247.Controllers
{
    public class ExternalLoginController : Controller
    {
        private readonly SignInManager<Movie247User> _signInManager;
        private readonly UserManager<Movie247User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginController> _logger;

        public ExternalLoginController(SignInManager<Movie247User> signInManager, UserManager<Movie247User> userManager, IEmailSender emailSender, ILogger<ExternalLoginController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        public async Task<IActionResult> OnPost(string provider, string returnUrl = null)
        {
            // get current url from  client
            returnUrl = returnUrl ?? HttpContext.Request.Path;
            // Kiểm tra yêu cầu dịch vụ provider tồn tại
            var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var provider_process = listprovider.Find((m) => m.Name == provider);
            if (provider_process == null)
            {
                return NotFound("Service not found" + provider);
            }
            // redirect to onGetCallbackAsync - sau khi gọi OnGetCallbackAsync thì sẽ chuyển hướng đến /dang-nhap-tu-google
            var redirectUrl = "/ExternalLogin/OnGetCallback";
            // Cấu hình
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            // Chuyển hướng đến dịch vụ ngoài (Googe, Facebook)
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"<h1 class='text-danger'>Error from external provider: {remoteError}</h1>";
                return RedirectToAction("Index", "Redirect", new { message = ErrorMessage });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "<h1 class='text-danger'>Error loading external login information</h1>";
                return RedirectToAction("Index", "Redirect", new { message = ErrorMessage });
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return Redirect("/");
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Index", "Redirect", new { message = "<h1 class='text-danger'>Your account is locked out</h1>" });
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                ViewData["Message"] = $"<h3 class='text-success'>You are logged in with {ProviderDisplayName}. Please set your Email address to Login.</h3>";
                return View("Index");

            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                // return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
                return new JsonResult(new { redirectUrl = Url.Content("~/") });
            }
            if (ModelState.IsValid)
            {
                var user = new Movie247User { UserName = Input.Email, Email = Input.Email };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, code = code }, protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            RedirectToAction("Index", "Redirect", new { redirectUrl = Url.Content("~/"), message = "Please check your email and confirm your account." });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                string errorMessage = "";
                foreach (var error in result.Errors)
                {
                    errorMessage += error.Description + " \n";
                }
                ErrorMessage = $"<h1 class='text-danger'>{errorMessage}</h1>";
            }
            else
            {
                ErrorMessage = "<h1 class='text-danger'>Email is wrong format</h1>";
            }
            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            ViewData["ErrorMessage"] = ErrorMessage;
            return View("Index");
        }

    }
}
