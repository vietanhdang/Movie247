﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie247.Models;

namespace Movie247.Controllers
{
    public class ProductionCompaniesController : Controller
    {
        private readonly MOVIEPROJECTContext _context;

        public ProductionCompaniesController(MOVIEPROJECTContext context)
        {
            _context = context;
        }

        // GET: ProductionCompanies
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductionCompanies.ToListAsync());
        }

        // GET: ProductionCompanies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionCompany = await _context.ProductionCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionCompany == null)
            {
                return NotFound();
            }

            return View(productionCompany);
        }

        // GET: ProductionCompanies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductionCompanies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,FilePath,Description,CreateAt,UpdateAt")] ProductionCompany productionCompany)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productionCompany);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productionCompany);
        }

        // GET: ProductionCompanies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionCompany = await _context.ProductionCompanies.FindAsync(id);
            if (productionCompany == null)
            {
                return NotFound();
            }
            return View(productionCompany);
        }

        // POST: ProductionCompanies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,FilePath,Description,CreateAt,UpdateAt")] ProductionCompany productionCompany)
        {
            if (id != productionCompany.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productionCompany);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductionCompanyExists(productionCompany.Id))
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
            return View(productionCompany);
        }

        // GET: ProductionCompanies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionCompany = await _context.ProductionCompanies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productionCompany == null)
            {
                return NotFound();
            }

            return View(productionCompany);
        }

        // POST: ProductionCompanies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productionCompany = await _context.ProductionCompanies.FindAsync(id);
            _context.ProductionCompanies.Remove(productionCompany);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductionCompanyExists(int id)
        {
            return _context.ProductionCompanies.Any(e => e.Id == id);
        }
    }
}
