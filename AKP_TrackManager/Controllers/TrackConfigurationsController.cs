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
    public class TrackConfigurationsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public TrackConfigurationsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<TrackConfiguration> PagedList = new X.PagedList.PagedList<TrackConfiguration>(await _context.TrackConfigurations.ToListAsync(), pageNumber, pageSize);
            return View(PagedList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackConfiguration = await _context.TrackConfigurations
                .FirstOrDefaultAsync(m => m.TrackId == id);
            if (trackConfiguration == null)
            {
                return NotFound();
            }

            return View(trackConfiguration);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackId,Reversable,Length,PresetName,PresetNumber,PresetImageLink")] TrackConfiguration trackConfiguration)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trackConfiguration);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trackConfiguration);
        }

        // GET: TrackConfigurations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackConfiguration = await _context.TrackConfigurations.FindAsync(id);
            if (trackConfiguration == null)
            {
                return NotFound();
            }
            return View(trackConfiguration);
        }

        // POST: TrackConfigurations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrackId,Reversable,Length,PresetName,PresetNumber,PresetImageLink")] TrackConfiguration trackConfiguration)
        {
            if (id != trackConfiguration.TrackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trackConfiguration);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackConfigurationExists(trackConfiguration.TrackId))
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
            return View(trackConfiguration);
        }

        // GET: TrackConfigurations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackConfiguration = await _context.TrackConfigurations
                .FirstOrDefaultAsync(m => m.TrackId == id);
            if (trackConfiguration == null)
            {
                return NotFound();
            }

            return View(trackConfiguration);
        }

        // POST: TrackConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trackConfiguration = await _context.TrackConfigurations.FindAsync(id);
            _context.TrackConfigurations.Remove(trackConfiguration);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackConfigurationExists(int id)
        {
            return _context.TrackConfigurations.Any(e => e.TrackId == id);
        }
    }
}
