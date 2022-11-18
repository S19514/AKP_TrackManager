using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using System.Xml.Linq;

namespace AKP_TrackManager.Controllers
{
    public class trainingsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public trainingsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.training.Include(t => t.LocationLocation).Include(t => t.TrackConfigurationTrack);
            var adc = aKP_TrackManager_devContext.ToList();
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }

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

        public IActionResult Create()
        {
            ViewData["LocationFriendlyName"] = new SelectList(_context.Locations, "LocationId", "FriendlyName");
            ViewData["TrackConfigurationPresetName"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetName");
            return View();
        }
        //public IActionResult SignUpForTraining()
        //{
        //    ViewData["TrainingDate"] = new SelectList(_context.training, "TrainingId", "Date");
        //    return View();
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> SingUpForTraining(int? id)
        {

            var training = _context.training.Where(t => t.TrainingId == id).FirstOrDefault();
            var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();
            var trainingAttandanceHistory = _context.TrainingAttandances
                                                    .Where(ta => ta.MemberMemberId == member.MemberId && ta.TrainingTrainingId == training.TrainingId)
                                                    .FirstOrDefault();
            if (trainingAttandanceHistory == null)
            {
                var trainingAttandance = new TrainingAttandance
                {
                    MemberMemberId = member.MemberId,
                    TrainingTrainingId = training.TrainingId,
                };
                _context.Add(trainingAttandance);
                await _context.SaveChangesAsync();
                var x = trainingAttandance.TrainingAttandanceId;
;
                return RedirectToAction("Index", "TrainingAttandances");
            }
            else 
                return RedirectToAction("Index", "TrainingAttandances");
        }

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
            ViewData["LocationFriendlyName"] = new SelectList(_context.Locations, "LocationId", "FriendlyName", training.LocationLocationId);
            ViewData["TrackConfigurationPresetName"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetName", training.TrackConfigurationTrackId);
            return View(training);
        }

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
            ViewData["LocationFriendlyName"] = new SelectList(_context.Locations, "LocationId", "FriendlyName", training.LocationLocationId);
            ViewData["TrackConfigurationPresetName"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetName", training.TrackConfigurationTrackId);
            return View(training);
        }

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
            ViewData["LocationFriendlyName"] = new SelectList(_context.Locations, "LocationId", "FriendlyName", training.LocationLocationId);
            ViewData["TrackConfigurationPresetName"] = new SelectList(_context.TrackConfigurations, "TrackId", "PresetName", training.TrackConfigurationTrackId);
            return View(training);
        }

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
