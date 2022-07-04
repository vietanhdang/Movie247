using Microsoft.AspNetCore.Mvc;

namespace Movie247.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
