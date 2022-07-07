using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Models;

namespace Movie247.Controllers
{
    public class MovieSourcesController : Controller
    {
        private readonly MOVIEPROJECTContext _context;

        public MovieSourcesController(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        // GET: MovieSources
        public async Task<IActionResult> Index()
        {
            var mOVIEPROJECTContext = _context.MovieSources.Include(m => m.Movie).OrderBy(x => x.Movie.Title);
            return View(await mOVIEPROJECTContext.ToListAsync());
        }

        // GET: MovieSources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieSource = await _context.MovieSources
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieSource == null)
            {
                return NotFound();
            }

            return View(movieSource);
        }

        // GET: MovieSources/Create
        public IActionResult Create()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title");

            return View();
        }

        // POST: MovieSources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MovieId,LinkSource,Description,CreateAt,UpdateAt")] MovieSource movieSource)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieSource);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieSource.MovieId);
            return View(movieSource);
        }

        // GET: MovieSources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieSource = await _context.MovieSources.FindAsync(id);
            if (movieSource == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieSource.MovieId);
            return View(movieSource);
        }

        // POST: MovieSources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,LinkSource,Description,CreateAt,UpdateAt")] MovieSource movieSource)
        {
            if (id != movieSource.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieSource);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieSourceExists(movieSource.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", movieSource.MovieId);
            return View(movieSource);
        }

        // GET: MovieSources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieSource = await _context.MovieSources
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieSource == null)
            {
                return NotFound();
            }

            return View(movieSource);
        }

        // POST: MovieSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieSource = await _context.MovieSources.FindAsync(id);
            _context.MovieSources.Remove(movieSource);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieSourceExists(int id)
        {
            return _context.MovieSources.Any(e => e.Id == id);
        }
    }
}
