using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie247.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Movie247.Logics;
namespace Movie247.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MOVIEPROJECTContext _context;

        public HomeController(ILogger<HomeController> logger, MOVIEPROJECTContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            PersonLogic personLogic = new(_context);
            MovieLogic movieLogic = new(_context);
            ViewData["top5TrailerLatest"] = movieLogic.Get5TrailerOfMovieLatest();
            ViewData["top12HighesRate"] = movieLogic.Get12MovieWithHighestRate();
            ViewData["top10PeopleHighestRate"] = personLogic.GetTop10People();
            return View();
        }

    }
}
