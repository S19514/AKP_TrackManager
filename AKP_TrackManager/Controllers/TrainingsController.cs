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
    public class trainingsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public trainingsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        // GET: trainings
        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.training.Include(t => t.LocationLocation).Include(t => t.TrackConfigurationTrack);
            var adc = aKP_TrackManager_devContext.ToList();
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }

        // GET: trainings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training
                .Include(t => t.LocationLocation)
                .Include(t => t.TrackConfigurationTrack)
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // GET: trainings/Create
        public IActionResult Create()
        {
            var tc = _context.TrackConfigurations.ToList();
            var lc = _context.Locations.ToList();
            ViewData["LocationFriendlyName"] = new SelectList(_context.Locations, "LocationId", "FriendlyName");
            ViewData["TrackConfigurationPresetName"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetName");
            return View();
        }

        // POST: trainings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingId,TrackConfigurationTrackId,Date,StartTime,EndTime,LocationLocationId")] training training)
        {
            if (ModelState.IsValid)
            {
                _context.Add(training);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationLocationId"] = new SelectList(_context.Locations, "LocationId", "Country", training.LocationLocationId);
            ViewData["TrackConfigurationTrackId"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetImageLink", training.TrackConfigurationTrackId);
            return View(training);
        }

        // GET: trainings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training.FindAsync(id);
            if (training == null)
            {
                return NotFound();
            }
            ViewData["LocationLocationId"] = new SelectList(_context.Locations, "LocationId", "Country", training.LocationLocationId);
            ViewData["TrackConfigurationTrackId"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetImageLink", training.TrackConfigurationTrackId);
            return View(training);
        }

        // POST: trainings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingId,TrackConfigurationTrackId,Date,StartTime,EndTime,LocationLocationId")] training training)
        {
            if (id != training.TrainingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(training);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!trainingExists(training.TrainingId))
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
            ViewData["LocationLocationId"] = new SelectList(_context.Locations, "LocationId", "Country", training.LocationLocationId);
            ViewData["TrackConfigurationTrackId"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetImageLink", training.TrackConfigurationTrackId);
            return View(training);
        }

        // GET: trainings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var training = await _context.training
                .Include(t => t.LocationLocation)
                .Include(t => t.TrackConfigurationTrack)
                .FirstOrDefaultAsync(m => m.TrainingId == id);
            if (training == null)
            {
                return NotFound();
            }

            return View(training);
        }

        // POST: trainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var training = await _context.training.FindAsync(id);
            _context.training.Remove(training);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool trainingExists(int id)
        {
            return _context.training.Any(e => e.TrainingId == id);
        }
    }
}
