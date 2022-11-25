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
    ///Generic controller with poor chance of being used
    ///
    public class MemberCarOnLapsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public MemberCarOnLapsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.MemberCarOnLaps.Include(m => m.CarCar).Include(m => m.LapLap).Include(m => m.MemberMember);
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberCarOnLap = await _context.MemberCarOnLaps
                .Include(m => m.CarCar)
                .Include(m => m.LapLap)
                .Include(m => m.MemberMember)
                .FirstOrDefaultAsync(m => m.MemberLapId == id);
            if (memberCarOnLap == null)
            {
                return NotFound();
            }

            return View(memberCarOnLap);
        }

        public IActionResult Create()
        {
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make");
            ViewData["LapLapId"] = new SelectList(_context.Laps, "LapId", "LapId");
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberLapId,MemberMemberId,CarCarId,LapLapId")] MemberCarOnLap memberCarOnLap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(memberCarOnLap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", memberCarOnLap.CarCarId);
            ViewData["LapLapId"] = new SelectList(_context.Laps, "LapId", "LapId", memberCarOnLap.LapLapId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", memberCarOnLap.MemberMemberId);
            return View(memberCarOnLap);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberCarOnLap = await _context.MemberCarOnLaps.FindAsync(id);
            if (memberCarOnLap == null)
            {
                return NotFound();
            }
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", memberCarOnLap.CarCarId);
            ViewData["LapLapId"] = new SelectList(_context.Laps, "LapId", "LapId", memberCarOnLap.LapLapId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", memberCarOnLap.MemberMemberId);
            return View(memberCarOnLap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberLapId,MemberMemberId,CarCarId,LapLapId")] MemberCarOnLap memberCarOnLap)
        {
            if (id != memberCarOnLap.MemberLapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(memberCarOnLap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberCarOnLapExists(memberCarOnLap.MemberLapId))
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
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", memberCarOnLap.CarCarId);
            ViewData["LapLapId"] = new SelectList(_context.Laps, "LapId", "LapId", memberCarOnLap.LapLapId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", memberCarOnLap.MemberMemberId);
            return View(memberCarOnLap);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memberCarOnLap = await _context.MemberCarOnLaps
                .Include(m => m.CarCar)
                .Include(m => m.LapLap)
                .Include(m => m.MemberMember)
                .FirstOrDefaultAsync(m => m.MemberLapId == id);
            if (memberCarOnLap == null)
            {
                return NotFound();
            }

            return View(memberCarOnLap);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var memberCarOnLap = await _context.MemberCarOnLaps.FindAsync(id);
            _context.MemberCarOnLaps.Remove(memberCarOnLap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberCarOnLapExists(int id)
        {
            return _context.MemberCarOnLaps.Any(e => e.MemberLapId == id);
        }
    }
}
