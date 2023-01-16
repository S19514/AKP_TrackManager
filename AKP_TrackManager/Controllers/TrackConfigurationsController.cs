using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrackConfigurationsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IConfigurationRepository _configurationRepository;

        public TrackConfigurationsController(AKP_TrackManager_devContext context, IConfigurationRepository configurationRepository)
        {
            _context = context;
            _configurationRepository = configurationRepository;
        }

        public async Task<IActionResult> Index(int? page, string searchName, int searchNumber)
        {
            return View(await _configurationRepository.Index(page,searchName,searchNumber));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var trackConfiguration = await _configurationRepository.Details(id);
            if(trackConfiguration == null)
                return RedirectToAction(nameof(Index));
          

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
                var newTrackConfiguration = await _configurationRepository.Create(trackConfiguration);
                if (newTrackConfiguration != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(trackConfiguration);
        }
        
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
                var updatedTrackConfiguration = await _configurationRepository.Edit(id, trackConfiguration);
                if(updatedTrackConfiguration != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(trackConfiguration);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           _=_configurationRepository.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
