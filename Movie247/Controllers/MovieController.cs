using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movie247.Helpers;
using Movie247.Logics;
using Movie247.Models;
using Movie247.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie247.Controllers
{

    public class MovieController : Controller
    {
        private readonly ILogger _logger;
        private readonly MOVIEPROJECTContext _context;

        public MovieController(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        public ViewResult List([Bind("CompanyId, CountryId, FromYear, ToYear, Name, GenreId, SortBy, OrderBy, Page, PageSize")] FilterModel filter)
        {
            MovieLogic movieLogic = new(_context);
            CompanyLogic companyLogic = new(_context);
            CountryLogic countryLogic = new(_context);
            GenreLogic genreLogic = new(_context);
            ViewData["Genres"] = genreLogic.GetAllGenresAsMultiSelectList();
            ViewData["Companies"] = companyLogic.GetAllCompaniesAsMultiSelectList();
            ViewData["Countries"] = countryLogic.GetAllCountriesAsMultiSelectList();
            ViewData["Movies"] = movieLogic.GetMovieByMultipleCondition(filter);
            ViewData["Filter"] = filter;
            ViewData["Paging"] = Pagination.Paging(Request.Path, Request.QueryString.ToString(), filter.Page, filter.TotalPages);
            return View("List");
        }

        // GET: MovieController/Details/5
        public ActionResult Details(int id)
        {
            MovieLogic movieLogic = new(_context);
            Movie movie = movieLogic.GetMovieById(id);
            ViewData["RelatedMovies"] = movieLogic.GetMovieInGenreById(movie.MovieGenres.ToList(), id);
            return View("Details", movie);
        }
        public ActionResult Watch(int id, int source)
        {

            MovieLogic movieLogic = new(_context);
            Movie movie = movieLogic.GetSourceMovieById(id);
            string sourceUrl = "";
            if (movie.MovieSources.Count > 0)
            {
                sourceUrl = movie.MovieSources.Take(1).FirstOrDefault().LinkSource;
            }
            else
            {
                sourceUrl = "https://www.youtube.com/watch?v=dQw4w9WgXcQ";
            }
            ViewData["Source"] = sourceUrl;
            ViewData["RelatedMovies"] = movieLogic.GetMovieInGenreById(movie.MovieGenres.ToList(), id);
            return View(movie);
        }

        // GET: MovieController/Create
        public ActionResult Create()
        {
            return View();
        }

    }
}
