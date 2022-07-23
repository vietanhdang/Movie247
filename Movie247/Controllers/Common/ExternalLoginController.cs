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
            returnUrl ??= Url.Content("~/");
            var listprovider = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var provider_process = listprovider.Find((m) => m.Name == provider);
            if (provider_process == null)
            {
                return NotFound("Service not found" + provider);
            }
            var redirectUrl = "/ExternalLogin/OnGetCallback?returnUrl=" + returnUrl;
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        [Route("/ExternalLogin/OnGetCallback")]
        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information";
                return RedirectToAction("Index", "Redirect", new { message = ErrorMessage });
            }
            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToAction("Index", "Redirect", new { message = "Your account is locked out" });
            }
            else
            {
                var userExisted = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (userExisted != null)
                {
                    // account existed, but not confirmed
                    if (!userExisted.EmailConfirmed)
                    {
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(userExisted);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = userExisted.Id, code = code }, protocol: Request.Scheme);
                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                        return RedirectToAction("Index", "Redirect", new { message = "Your account is not activated. We have sent you an activation link. Please check your email." });
                    }
                }
                else
                {
                    var user = await _userManager.FindByEmailAsync(info.Principal.FindFirstValue(ClaimTypes.Email));
                    if (user != null)
                    {
                        // create login for user
                        await _userManager.AddLoginAsync(user, info);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        if (info.ProviderDisplayName == "Google")
                        {
                            var user1 = new Movie247User
                            {
                                UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                                Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                                Firstname = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                                EmailConfirmed = true
                            };
                            await _userManager.CreateAsync(user1);
                            await _userManager.AddLoginAsync(user1, info);
                            await _signInManager.SignInAsync(user1, isPersistent: false);
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            // if provider is google, create user
                            ProviderDisplayName = info.ProviderDisplayName;
                            if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                            {
                                Input = new InputModel
                                {
                                    Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                                };
                            }
                            ViewData["Message"] = $"You are logged in with {ProviderDisplayName}. Please link your account to your {ProviderDisplayName} account.";
                        }
                    }
                }
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
                RedirectToAction("Index", "Redirect", new { message = "Error loading external login information during confirmation." });
            }
            if (ModelState.IsValid)
            {
                var user = new Movie247User { UserName = Input.Email, Email = Input.Email, Firstname = info.Principal.Identity.Name };
                var findUserByEmail = await _userManager.FindByEmailAsync(Input.Email);
                if (findUserByEmail != null)
                {
                    ViewData["Message"] = $"Email {Input.Email} has already been used. Please try another email.";
                    return View("Index");
                }
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
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Scheme);

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
                ErrorMessage = $"{errorMessage}";
            }
            else
            {
                ErrorMessage = "Email is wrong format";
            }
            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            ViewData["ErrorMessage"] = ErrorMessage;
            return View("Index");
        }

    }
}
