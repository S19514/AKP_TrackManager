using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;

namespace AKP_TrackManager.Controllers
{
    public class AccidentsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public AccidentsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        // GET: Accidents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accidents.ToListAsync());
        }

        // GET: Accidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accident = await _context.Accidents
                .FirstOrDefaultAsync(m => m.AccidentId == id);
            if (accident == null)
            {
                return NotFound();
            }

            return View(accident);
        }

        // GET: Accidents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Accidents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccidentId,AccidentDate,Severity,AnyoneInjured")] Accident accident)
        {
            if (ModelState.IsValid)
            {
                _context.Add(accident);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(accident);
        }

        // GET: Accidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accident = await _context.Accidents.FindAsync(id);
            if (accident == null)
            {
                return NotFound();
            }
            return View(accident);
        }

        // POST: Accidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccidentId,AccidentDate,Severity,AnyoneInjured")] Accident accident)
        {
            if (id != accident.AccidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccidentExists(accident.AccidentId))
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
            return View(accident);
        }

        // GET: Accidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accident = await _context.Accidents
                .FirstOrDefaultAsync(m => m.AccidentId == id);
            if (accident == null)
            {
                return NotFound();
            }

            return View(accident);
        }

        // POST: Accidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accident = await _context.Accidents.FindAsync(id);
            _context.Accidents.Remove(accident);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccidentExists(int id)
        {
            return _context.Accidents.Any(e => e.AccidentId == id);
        }
    }
}
