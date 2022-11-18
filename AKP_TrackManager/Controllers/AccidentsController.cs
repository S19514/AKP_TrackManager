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
    public class AccidentsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public AccidentsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        // GET: Accidents
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accidents.ToListAsync());
        }

        // GET: Accidents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accident = await _context.Accidents.FirstOrDefaultAsync(m => m.AccidentId == id);
            var carAccidentByMember = await _context.CarAccidentByMembers.FirstOrDefaultAsync(m=>m.AccidentAccidentId==id);
            var car = await _context.Cars.FirstOrDefaultAsync(m => m.CarId == carAccidentByMember.CarCarId);
            var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberId == carAccidentByMember.MemberMemberId);
            var accidentCarMemberDto = new AccidentCarMemberDto()
            {
                AccidentDate = accident.AccidentDate,
                AccidentId = accident.AccidentId,
                AnyoneInjured = accident.AnyoneInjured,
                Severity = accident.Severity,
                CarId = car.CarId,
                MemberId = member.MemberId,
                EmailAddress = member.EmailAddress,
                RegPlate = car.RegPlate
            };


            if (accident == null)
            {
                return NotFound();
            }

            return View(accidentCarMemberDto);
        }

        // GET: Accidents/Create
        public IActionResult Create()
        {
            ViewData["EmailAddress"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            ViewData["RegPlate"] = new SelectList(_context.Cars, "CarId", "RegPlate");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccidentId,AccidentDate,Severity,AnyoneInjured,MemberId,CarId")] AccidentCarMemberDto accident)
        {
            if (ModelState.IsValid)
            {
                var postAccident = new Accident()
                {
                    AccidentDate = accident.AccidentDate,
                    AnyoneInjured = accident.AnyoneInjured,
                    Severity = accident.Severity
                };
                _context.Add(postAccident);
                await _context.SaveChangesAsync();

                var carAccidentByMember = new CarAccidentByMember()
                {
                    AccidentAccidentId = postAccident.AccidentId,
                    CarCarId = accident.CarId,
                    MemberMemberId = accident.MemberId
                };
                _context.Add(carAccidentByMember);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(accident);
        }

        // GET: Accidents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accident = await _context.Accidents.FindAsync(id);
            if (accident == null)
            {
                return NotFound();
            }
            return View(accident);
        }

        // POST: Accidents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccidentId,AccidentDate,Severity,AnyoneInjured")] Accident accident)
        {
            if (id != accident.AccidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(accident);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccidentExists(accident.AccidentId))
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
            return View(accident);
        }

        // GET: Accidents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accident = await _context.Accidents
                .FirstOrDefaultAsync(m => m.AccidentId == id);
            if (accident == null)
            {
                return NotFound();
            }

            return View(accident);
        }

        // POST: Accidents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accident = await _context.Accidents.FindAsync(id);
            var carAccidentByMember = await _context.CarAccidentByMembers.Where(cabm => cabm.AccidentAccidentId == accident.AccidentId).FirstOrDefaultAsync();
            _context.CarAccidentByMembers.Remove(carAccidentByMember);
            _context.Accidents.Remove(accident);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccidentExists(int id)
        {
            return _context.Accidents.Any(e => e.AccidentId == id);
        }
    }
}
