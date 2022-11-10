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
    public class LapsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public LapsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        // GET: Laps
        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.Laps.Include(l => l.TrainingTraining);
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }

        // GET: Laps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lap = await _context.Laps
                .Include(l => l.TrainingTraining)
                .FirstOrDefaultAsync(m => m.LapId == id);
            if (lap == null)
            {
                return NotFound();
            }

            return View(lap);
        }

        // GET: Laps/Create
        public IActionResult Create()
        {
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId");
            return View();
        }

        // POST: Laps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LapId,MeasuredTime,PenaltyTime,AbsoluteTime,TrainingTrainingId")] Lap lap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
            return View(lap);
        }

        // GET: Laps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lap = await _context.Laps.FindAsync(id);
            if (lap == null)
            {
                return NotFound();
            }
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
            return View(lap);
        }

        // POST: Laps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LapId,MeasuredTime,PenaltyTime,AbsoluteTime,TrainingTrainingId")] Lap lap)
        {
            if (id != lap.LapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LapExists(lap.LapId))
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
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
            return View(lap);
        }

        // GET: Laps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lap = await _context.Laps
                .Include(l => l.TrainingTraining)
                .FirstOrDefaultAsync(m => m.LapId == id);
            if (lap == null)
            {
                return NotFound();
            }

            return View(lap);
        }

        // POST: Laps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lap = await _context.Laps.FindAsync(id);
            _context.Laps.Remove(lap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LapExists(int id)
        {
            return _context.Laps.Any(e => e.LapId == id);
        }
    }
}
