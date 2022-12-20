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
    [Authorize]
    public class TrainingAttandancesController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IAttendanceRepository _attendanceRepository;
        public TrainingAttandancesController(AKP_TrackManager_devContext context,IAttendanceRepository attendanceRepository)
        {
            _context = context;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var attendances = await _attendanceRepository.Index(page, User.Identity.Name, User.IsInRole("Admin"));         
            return View(attendances);
            
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {                            
                return View("IndexAdmin", await _attendanceRepository.IndexFilterAdmin(page, User.Identity.Name));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingAttendance = await _attendanceRepository.Details(id, User.Identity.Name, User.IsInRole("Admin"));
            if(trainingAttendance==null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(trainingAttendance);
        }
      

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingAttandance = await _context.TrainingAttandances
                .Include(t => t.MemberMember)
                .Include(t => t.TrainingTraining)
                .Include(t => t.TrainingTraining.LocationLocation)
                .FirstOrDefaultAsync(m => m.TrainingAttandanceId == id);
            if (trainingAttandance == null)
            {
                return NotFound();
            }

            return View(trainingAttandance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingId = await _attendanceRepository.DeleteConfirmed(id, User.Identity.Name, User.IsInRole("Admin"));
            if (trainingId > 0)
            {
                return RedirectToAction("Details", "Trainings", new { id = trainingId });
            }
            return RedirectToAction("Index", "Trainings");
        }

        #region not used
        //public IActionResult Create()
        //{
        //    ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
        //    ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId");
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("TrainingAttandanceId,TrainingTrainingId,MemberMemberId")] TrainingAttandance trainingAttandance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(trainingAttandance);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", trainingAttandance.MemberMemberId);
        //    ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", trainingAttandance.TrainingTrainingId);
        //    return View(trainingAttandance);
        //}
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var trainingAttandance = await _context.TrainingAttandances.FindAsync(id);
        //    if (trainingAttandance == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", trainingAttandance.MemberMemberId);
        //    ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", trainingAttandance.TrainingTrainingId);
        //    return View(trainingAttandance);
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("TrainingAttandanceId,TrainingTrainingId,MemberMemberId")] TrainingAttandance trainingAttandance)
        //{
        //    if (id != trainingAttandance.TrainingAttandanceId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(trainingAttandance);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TrainingAttandanceExists(trainingAttandance.TrainingAttandanceId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", trainingAttandance.MemberMemberId);
        //    ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", trainingAttandance.TrainingTrainingId);
        //    return View(trainingAttandance);
        //}

        #endregion

    }
}
