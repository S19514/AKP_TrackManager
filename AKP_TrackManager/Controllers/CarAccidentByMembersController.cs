using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;

///Generic controller with poor chance of being used
///
namespace AKP_TrackManager.Controllers
{
    public class CarAccidentByMembersController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public CarAccidentByMembersController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.CarAccidentByMembers.Include(c => c.AccidentAccident).Include(c => c.CarCar).Include(c => c.MemberMember);
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carAccidentByMember = await _context.CarAccidentByMembers
                .Include(c => c.AccidentAccident)
                .Include(c => c.CarCar)
                .Include(c => c.MemberMember)
                .FirstOrDefaultAsync(m => m.CarAccidentMemberId == id);
            if (carAccidentByMember == null)
            {
                return NotFound();
            }

            return View(carAccidentByMember);
        }

        public IActionResult Create()
        {
            ViewData["AccidentAccidentId"] = new SelectList(_context.Accidents, "AccidentId", "AccidentId");
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make");
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarAccidentMemberId,MemberMemberId,CarCarId,AccidentAccidentId")] CarAccidentByMember carAccidentByMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carAccidentByMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccidentAccidentId"] = new SelectList(_context.Accidents, "AccidentId", "AccidentId", carAccidentByMember.AccidentAccidentId);
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", carAccidentByMember.CarCarId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", carAccidentByMember.MemberMemberId);
            return View(carAccidentByMember);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carAccidentByMember = await _context.CarAccidentByMembers.FindAsync(id);
            if (carAccidentByMember == null)
            {
                return NotFound();
            }
            ViewData["AccidentAccidentId"] = new SelectList(_context.Accidents, "AccidentId", "AccidentId", carAccidentByMember.AccidentAccidentId);
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", carAccidentByMember.CarCarId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", carAccidentByMember.MemberMemberId);
            return View(carAccidentByMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarAccidentMemberId,MemberMemberId,CarCarId,AccidentAccidentId")] CarAccidentByMember carAccidentByMember)
        {
            if (id != carAccidentByMember.CarAccidentMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carAccidentByMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarAccidentByMemberExists(carAccidentByMember.CarAccidentMemberId))
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
            ViewData["AccidentAccidentId"] = new SelectList(_context.Accidents, "AccidentId", "AccidentId", carAccidentByMember.AccidentAccidentId);
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", carAccidentByMember.CarCarId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", carAccidentByMember.MemberMemberId);
            return View(carAccidentByMember);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carAccidentByMember = await _context.CarAccidentByMembers
                .Include(c => c.AccidentAccident)
                .Include(c => c.CarCar)
                .Include(c => c.MemberMember)
                .FirstOrDefaultAsync(m => m.CarAccidentMemberId == id);
            if (carAccidentByMember == null)
            {
                return NotFound();
            }

            return View(carAccidentByMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carAccidentByMember = await _context.CarAccidentByMembers.FindAsync(id);
            _context.CarAccidentByMembers.Remove(carAccidentByMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarAccidentByMemberExists(int id)
        {
            return _context.CarAccidentByMembers.Any(e => e.CarAccidentMemberId == id);
        }
    }
}
