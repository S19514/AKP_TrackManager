using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;

namespace AKP_TrackManager.Controllers
{
    public class TrainingAttandancesController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public TrainingAttandancesController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {
            
                if (HttpContext.User.IsInRole("Admin"))
                {                   
                    var aKP_TrackManager_devContext = _context.TrainingAttandances
                                                                .Include(t => t.MemberMember)
                                                                .Include(t => t.TrainingTraining)
                                                                .Include(t => t.TrainingTraining.LocationLocation);
                    //return View(await aKP_TrackManager_devContext.ToListAsync());
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<TrainingAttandance> PagedList = new X.PagedList.PagedList<TrainingAttandance>(aKP_TrackManager_devContext, pageNumber, pageSize);
                return View(PagedList);
            }
                else
                {
                    var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();
                    var aKP_TrackManager_devContext = _context.TrainingAttandances
                                                                .Include(t => t.MemberMember)
                                                                .Include(t => t.TrainingTraining)
                                                                .Include(t => t.TrainingTraining.LocationLocation)
                                                                .Where(t => t.MemberMemberId == member.MemberId);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<TrainingAttandance> PagedList = new X.PagedList.PagedList<TrainingAttandance>(aKP_TrackManager_devContext, pageNumber, pageSize);
                return View(PagedList);
            }                        
        }

        [System.Web.Http.Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();
                var aKP_TrackManager_devContext = _context.TrainingAttandances
                                                            .Include(t => t.MemberMember)
                                                            .Include(t => t.TrainingTraining)
                                                            .Include(t => t.TrainingTraining.LocationLocation)
                                                            .Where(t => t.MemberMemberId == member.MemberId);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<TrainingAttandance> PagedList = new X.PagedList.PagedList<TrainingAttandance>(aKP_TrackManager_devContext, pageNumber, pageSize);
                return View("IndexAdmin",PagedList);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> Details(int? id)
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

        public IActionResult Create()
        {
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrainingAttandanceId,TrainingTrainingId,MemberMemberId")] TrainingAttandance trainingAttandance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingAttandance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", trainingAttandance.MemberMemberId);
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", trainingAttandance.TrainingTrainingId);
            return View(trainingAttandance);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingAttandance = await _context.TrainingAttandances.FindAsync(id);
            if (trainingAttandance == null)
            {
                return NotFound();
            }
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", trainingAttandance.MemberMemberId);
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", trainingAttandance.TrainingTrainingId);
            return View(trainingAttandance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TrainingAttandanceId,TrainingTrainingId,MemberMemberId")] TrainingAttandance trainingAttandance)
        {
            if (id != trainingAttandance.TrainingAttandanceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingAttandance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingAttandanceExists(trainingAttandance.TrainingAttandanceId))
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
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", trainingAttandance.MemberMemberId);
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", trainingAttandance.TrainingTrainingId);
            return View(trainingAttandance);
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
            var trainingAttandance = await _context.TrainingAttandances.FindAsync(id);
            var trainingId = trainingAttandance.TrainingTrainingId;
            _context.TrainingAttandances.Remove(trainingAttandance);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Trainings", new { id = trainingId });
        }

        private bool TrainingAttandanceExists(int id)
        {
            return _context.TrainingAttandances.Any(e => e.TrainingAttandanceId == id);
        }
    }
}
