﻿using Microsoft.AspNetCore.Mvc;

namespace Movie247.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult Index(string message, string ActionLinkFromController, string title, string returnUrl)
        {
            if (message == null && ActionLinkFromController == null && title == null && returnUrl == null)
            {
                return Redirect("/");
            }
            ViewData["message"] = message;
            ViewData["ActionLinkFromController"] = ActionLinkFromController;
            ViewData["title"] = title;
            ViewData["returnUrl"] = returnUrl;
            return View("_Redirect");
        }
        public IActionResult AccessDenied(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View("_AccessDenied");
        }
        [Route("/Redirect/{statusCode}")]
        public IActionResult ErrorPage(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    ViewData["message"] = "Page Not Found";
                    ViewData["title"] = "Page Not Found";
                    break;
                default:
                    break;
            }

            return View("_Redirect");
        }
    }
}
