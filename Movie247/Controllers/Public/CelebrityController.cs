using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Helpers;
using Movie247.Logics;
using Movie247.Utils;
using Movie247.Data;
using Movie247.Models;

namespace Movie247.Controllers
{
    public class CelebrityController : Controller
    {
        public readonly Movie247Context _context;

        public CelebrityController(Movie247Context context)
        {
            _context = context;
        }

        // GET: Celebrity
        public async Task<IActionResult> List([Bind("Keyword, SortBy, FromYear, Page, ToYear, Category, OrderBy, PageSize")] FilterModel filter)
        {
            List<Person> celebrities = await new PersonLogic().FindPersonByFilter(filter);
            ViewData["Top10Celebrities"] = await new PersonLogic().GetTop10People();
            ViewData["DateFromAndTo"] = new PersonLogic().GetLatestAndOldestYearOfPerson();
            ViewData["Filter"] = filter;
            ViewData["Paging"] = Pagination.Paging(Request.Path, Request.QueryString.ToString(), filter.Page, filter.TotalPages);
            return View("List", celebrities);
        }

        // GET: Celebrity/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var person = new PersonLogic().FindPersonById(id ?? 0);

            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }
    }
}
