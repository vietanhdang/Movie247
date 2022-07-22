using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Movie247.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Movie247.Logics;
using Movie247.Data;

namespace Movie247.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Movie247Context _context;

        public HomeController(ILogger<HomeController> logger, Movie247Context context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["top5TrailerLatest"] = await new MovieLogic().Get5TrailerOfMovieLatest();
            ViewData["top12NewestMovies"] = await new MovieLogic().Get12MovieNewest();
            ViewData["top12TopRatedMovies"] = await new MovieLogic().Get12TopRatedMovies();
            ViewData["top10PeopleHighestRate"] = await new PersonLogic().GetTop10People();
            ViewData["top12MovieFavoriteOfUserMost"] = await new MovieLogic().Get12MovieFavoriteOfUserMost();
            ViewData["top12MovieMostViewedOfUserMost"] = await new MovieLogic().Get12MovieMostViewedOfUserMost();
            return View();
        }
        public IActionResult Error()
        {
            return View("_Redirect");
        }

    }
}
