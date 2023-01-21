using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;
using AKP_TrackManager.Repository;

namespace AKP_TrackManager.Controllers
{
    [Authorize]
    public class trainingsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private ITrainingRepository _trainingRepository;

        public trainingsController(AKP_TrackManager_devContext context, ITrainingRepository trainingRepository)
        {
            _context = context;
            _trainingRepository = trainingRepository;
        }

        public async Task<IActionResult> Index(int? page, DateTime? searchString)
        {
            if (searchString != null)
            {
                DateTime dateTime = Convert.ToDateTime(searchString);
                ViewBag.SearchString = dateTime.ToString("yyy-MM-dd");
            }


            return View(await _trainingRepository.Index(page,searchString));            
        }

        public async Task<IActionResult> Details(int? id)
        {
            var training = await _trainingRepository.Details(id);
            if (training == null)
                return NotFound();
            else
                return View(training);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["LocationFriendlyName"] = _trainingRepository.GetLocationFriendlyNames() ;
            ViewData["TrackConfigurationPresetName"] = _trainingRepository.GetTrackConfigurationPresetNames() ;
            return View();
        }

        public async Task<IActionResult> SingUpForTraining(int? id)
        {
         if(await _trainingRepository.SingUpForTraining(id,User.Identity.Name))
            { 
                return RedirectToAction("Details", "Trainings", new {id = id});
            }
            else
            {
                return RedirectToAction("Details", "Trainings", new { id = id });
                //possible to display info about being already signed for this training
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingId,TrackConfigurationTrackId,Date,StartTime,EndTime,LocationLocationId")] training training)
        {
            if (ModelState.IsValid)
            {
                var newTraining = await _trainingRepository.Create(new Models.DTO.TrainingCreateDto { Training = training });
                if (newTraining.Training == null)
                    return RedirectToAction(nameof(Index));
                else
                    ModelState.AddModelError(newTraining.ModelErrorKey, newTraining.ModelErrorString);
            }
            ViewData["LocationFriendlyName"] = _trainingRepository.GetSelectedLocationFriendlyNames(training.LocationLocationId);
            ViewData["TrackConfigurationPresetName"] = _trainingRepository.GetSelectedTrackConfigurationPresetNames(training.TrackConfigurationTrackId);
            return View(training);
        }

        [Authorize(Roles = "Admin")]
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
            ViewData["LocationFriendlyName"] = _trainingRepository.GetSelectedLocationFriendlyNames(training.LocationLocationId);
            ViewData["TrackConfigurationPresetName"] = _trainingRepository.GetSelectedTrackConfigurationPresetNames(training.TrackConfigurationTrackId);
            return View(training);
        }

        [Authorize(Roles = "Admin")]
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
                if(await _trainingRepository.Edit(id, training) != null)
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationFriendlyName"] = _trainingRepository.GetSelectedLocationFriendlyNames(training.LocationLocationId);
            ViewData["TrackConfigurationPresetName"] = _trainingRepository.GetSelectedTrackConfigurationPresetNames(training.TrackConfigurationTrackId);
            return View(training);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _trainingRepository.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
