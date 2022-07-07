using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Helpers;
using Movie247.Logics;
using Movie247.Models;
using Movie247.Utils;

namespace Movie247.Controllers
{
    public class CelebrityController : Controller
    {
        public readonly MOVIEPROJECTContext _context;

        public CelebrityController(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        // GET: Celebrity
        public IActionResult List([Bind("Keyword, SortBy, FromYear, Page, ToYear, Category, OrderBy, PageSize")] FilterModel filter)
        {
            PersonLogic personLogic = new(_context);
            List<Person> celebrities = personLogic.FindPersonByFilter(filter);
            ViewData["Top10Celebrities"] = personLogic.GetTop10People();
            ViewData["DateFromAndTo"] = personLogic.GetLatestAndOldestYearOfPerson();
            ViewData["Filter"] = filter;
            ViewData["Paging"] = Pagination.Paging(Request.Path, Request.QueryString.ToString(), filter.Page, filter.TotalPages);
            return View("List", celebrities);
        }

        // GET: Celebrity/Details/5
        public IActionResult Details(int? id)
        {
            PersonLogic personLogic = new(_context);
            if (id == null)
            {
                return NotFound();
            }
            var person = personLogic.FindPersonById(id ?? 0);

            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // GET: Celebrity/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Celebrity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ProfilePath,Description,CreateAt,UpdateAt,Popularity,Birthday")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Celebrity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            return View(person);
        }

        // POST: Celebrity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ProfilePath,Description,CreateAt,UpdateAt,Popularity,Birthday")] Person person)
        {
            if (id != person.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(person.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        // GET: Celebrity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        // POST: Celebrity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.People.FindAsync(id);
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(int id)
        {
            return _context.People.Any(e => e.Id == id);
        }
    }
}
