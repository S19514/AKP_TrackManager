using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private ILocationRepository _locationRepository;
        public LocationsController(AKP_TrackManager_devContext context, ILocationRepository locationRepository)
        {
            _context = context;
            _locationRepository = locationRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {                                                           
            return View(await _locationRepository.Index(page));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var locationDetails = await _locationRepository.Details(id);
            if (locationDetails == null)
                return RedirectToAction(nameof(Index));
           
            return View(locationDetails);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,FriendlyName,Town,Street,Country")] Location location)
        {
            if (ModelState.IsValid)
            {
                var newLocation = await _locationRepository.Create(location);
                if(newLocation != null)
                    return RedirectToAction(nameof(Index));
            }
            return View(location);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,FriendlyName,Town,Street,Country")] Location location)
        {
            if (id != location.LocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedLocation = await _locationRepository.Edit(id, location);
                if(updatedLocation != null)
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           _= await _locationRepository.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}
