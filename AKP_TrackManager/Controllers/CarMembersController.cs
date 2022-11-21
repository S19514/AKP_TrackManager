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
    public class CarMembersController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public CarMembersController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.CarMembers.Include(c => c.CarCar).Include(c => c.MemberMember);
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carMember = await _context.CarMembers
                .Include(c => c.CarCar)
                .Include(c => c.MemberMember)
                .FirstOrDefaultAsync(m => m.CarMemberId == id);
            if (carMember == null)
            {
                return NotFound();
            }

            return View(carMember);
        }

        public IActionResult Create()
        {
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make");
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberMemberId,CarCarId,CarMemberId")] CarMember carMember)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", carMember.CarCarId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", carMember.MemberMemberId);
            return View(carMember);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carMember = await _context.CarMembers.FindAsync(id);
            if (carMember == null)
            {
                return NotFound();
            }
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", carMember.CarCarId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", carMember.MemberMemberId);
            return View(carMember);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberMemberId,CarCarId,CarMemberId")] CarMember carMember)
        {
            if (id != carMember.CarMemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarMemberExists(carMember.CarMemberId))
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
            ViewData["CarCarId"] = new SelectList(_context.Cars, "CarId", "Make", carMember.CarCarId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", carMember.MemberMemberId);
            return View(carMember);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carMember = await _context.CarMembers
                .Include(c => c.CarCar)
                .Include(c => c.MemberMember)
                .FirstOrDefaultAsync(m => m.CarMemberId == id);
            if (carMember == null)
            {
                return NotFound();
            }

            return View(carMember);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carMember = await _context.CarMembers.FindAsync(id);
            _context.CarMembers.Remove(carMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarMemberExists(int id)
        {
            return _context.CarMembers.Any(e => e.CarMemberId == id);
        }
    }
}
