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
    public class TrainingAttandancesController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public TrainingAttandancesController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            
                if (HttpContext.User.IsInRole("Admin"))
                {                   
                    var aKP_TrackManager_devContext = _context.TrainingAttandances
                                                                .Include(t => t.MemberMember)
                                                                .Include(t => t.TrainingTraining)
                                                                .Include(t => t.TrainingTraining.LocationLocation);
                    return View(await aKP_TrackManager_devContext.ToListAsync());
                }
                else
                {
                    var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();
                    var aKP_TrackManager_devContext = _context.TrainingAttandances
                                                                .Include(t => t.MemberMember)
                                                                .Include(t => t.TrainingTraining)
                                                                .Include(t => t.TrainingTraining.LocationLocation)
                                                                .Where(t => t.MemberMemberId == member.MemberId);
                    return View(await aKP_TrackManager_devContext.ToListAsync());
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
            _context.TrainingAttandances.Remove(trainingAttandance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingAttandanceExists(int id)
        {
            return _context.TrainingAttandances.Any(e => e.TrainingAttandanceId == id);
        }
    }
}
