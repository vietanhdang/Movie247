using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movie247.Helpers;
using Movie247.Logics;
using Movie247.Models;
using Movie247.Utils;
using Movie247.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Movie247.Areas.Identity.Data;
using Microsoft.Extensions.Configuration;

namespace Movie247.Controllers
{

    public class MovieController : Controller
    {
        private readonly ILogger<MovieController> _logger;
        private readonly Movie247Context _context;
        private readonly UserManager<Movie247User> _userManager;
        private readonly SignInManager<Movie247User> _signInManager;
        private readonly IConfiguration _configuration;

        public MovieController(ILogger<MovieController> logger, Movie247Context context, UserManager<Movie247User> userManager, SignInManager<Movie247User> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public async Task<IActionResult> List([Bind("CompanyId, CountryId, FromYear, ToYear, Name, GenreId, SortBy, OrderBy, Page, PageSize")] FilterModel filter)
        {
            _logger.LogInformation("Home Page");
            ViewData["Genres"] = await new GenreLogic().GetAllGenresAsMultiSelectList();
            ViewData["Companies"] = await new CompanyLogic().GetAllCompaniesAsMultiSelectList();
            ViewData["Countries"] = await new CountryLogic().GetAllCountriesAsMultiSelectList();
            ViewData["Movies"] = await new MovieLogic().GetMovieByMultipleCondition(filter);
            ViewData["Filter"] = filter;
            ViewData["Paging"] = Pagination.Paging(Request.Path, Request.QueryString.ToString(), filter.Page, filter.TotalPages);
            return View("List");
        }

        public async Task<IActionResult> Details(int id)
        {
            var config = _configuration.GetSection("LoadReview");
            int loadNumberOfReview = config.GetSection("loadNumberOfReview").Get<int>();
            string SortBy = config.GetSection("SortBy").Get<string>();
            string OrderBy = config.GetSection("OrderBy").Get<string>();
            Movie movie = new MovieLogic().GetMovieById(id);
            if (movie == null)
            {
                ViewData["message"] = "Movie not found";
                ViewData["ActionLinkFromController"] = "<a href='/Movie/List' class='redbtn'>Back to Movies</a>";
                return View("_Redirect");
            }
            ViewData["TotalCommentCount"] = await new MovieLogic().GetTotalCommentMovieId(id);
            ViewData["RelatedMovies"] = new MovieLogic().GetMovieInGenreById(movie.MovieGenres.ToList(), id);
            ViewData["GetTotalReviews"] = await new MovieLogic().GetTotalReviews(id);
            ViewData["GetReviews"] = new MovieLogic().GetReviewByMovieId(id, 1, loadNumberOfReview, SortBy, OrderBy);
            return View("Details", movie);
        }

        [Authorize(Roles = "Watch")]
        public async Task<IActionResult> Watch(int id)
        {
            Movie movie = new MovieLogic().GetSourceMovieById(id);
            if (movie == null)
            {
                ViewData["message"] = "Movie not found";
                ViewData["ActionLinkFromController"] = "<a href='/Movie/List' class='redbtn'>Back to Movies</a>";
                return View("_Redirect");
            }
            if (movie.MovieSources.Count == 0)
            {
                return RedirectToAction("Details", "Movie", new { id = id });
            }
            var config = _configuration.GetSection("LoadReview");
            int loadNumberOfReview = config.GetSection("loadNumberOfReview").Get<int>();
            string SortBy = config.GetSection("SortBy").Get<string>();
            string OrderBy = config.GetSection("OrderBy").Get<string>();
            var movieReview = await new MovieLogic().GetReviewByUserId(id, _userManager.GetUserId(User));
            ViewData["TotalCommentCount"] = await new MovieLogic().GetTotalCommentMovieId(id);
            ViewData["GetTotalReviews"] = await new MovieLogic().GetTotalReviews(id);
            ViewData["GetReviews"] = new MovieLogic().GetReviewByMovieId(id, 1, loadNumberOfReview, SortBy, OrderBy);
            ViewData["UserId"] = _userManager.GetUserId(User);
            ViewData["MovieComments"] = await new MovieLogic().GetCommentMovieId(id);
            ViewData["ReviewdMovies"] = movieReview;
            ViewData["RelatedMovies"] = new MovieLogic().GetMovieInGenreById(movie.MovieGenres.ToList(), id);
            return View(movie);
        }
        public JsonResult LoadMoreReview(int movieId, int page)
        {
            var config = _configuration.GetSection("LoadReview");
            int loadNumberOfReview = config.GetSection("loadNumberOfReview").Get<int>();
            string SortBy = config.GetSection("SortBy").Get<string>();
            string OrderBy = config.GetSection("OrderBy").Get<string>();
            var comments = new MovieLogic().GetReviewByMovieId(movieId, page + 1, loadNumberOfReview, SortBy, OrderBy);
            return JsonReturn.Success("", comments);
        }
        public JsonResult AddWatchedTime(int movieId, int currentTime)
        {
            var cookie = Request.Cookies["watchedTime"];
            if (cookie == null)
            {
                cookie = JsonConvert.SerializeObject(new Dictionary<int, int>());
            }
            var watchedTime = JsonConvert.DeserializeObject<Dictionary<int, int>>(cookie);
            if (watchedTime.ContainsKey(movieId))
            {
                watchedTime[movieId] = currentTime;
            }
            else
            {
                watchedTime.Add(movieId, currentTime);
            }
            Response.Cookies.Append("watchedTime", JsonConvert.SerializeObject(watchedTime));
            return Json(new { success = true });
        }
        public JsonResult GetWatchedTime(int movieId)
        {
            var cookie = Request.Cookies["watchedTime"];
            if (cookie == null)
            {
                cookie = JsonConvert.SerializeObject(new Dictionary<int, int>());
            }
            var watchedTime = JsonConvert.DeserializeObject<Dictionary<int, int>>(cookie);
            if (watchedTime.ContainsKey(movieId))
            {
                return Json(new { success = true, lastTimeWatch = watchedTime[movieId] });
            }
            return JsonReturn.Error("");
        }

        public JsonResult UpdateView(int movieId)
        {
            // find movie by id
            Movie movie = new MovieLogic().GetMovieFirstById(movieId);
            if (movie != null)
            {
                // update view count
                if (new MovieLogic().UpdateView(movie) != 0)
                {
                    return Json(new { success = true });
                }
            }
            else
            {
                return JsonReturn.Success("");
            }
            return JsonReturn.Error("");
        }
    }
}
