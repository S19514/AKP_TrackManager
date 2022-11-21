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
    public class CarsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public CarsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                return View(await _context.Cars.ToListAsync());
            }
            else
            {
                List<CarMemberDto> carMemberDtos= new List<CarMemberDto>();
                var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();                                
                //var cars = _context.Cars.Include(t=>t.CarMembers).Where(c=>c.CarMembers.First().)

                //todoFilterThisList
                return View(await _context.Cars.ToListAsync());
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public IActionResult Create()
        {            
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Make,Model,EngingeCapacity,EnginePower,RegPlate,MemberId")] CarMemberDto pCarMember)
        {
            if (ModelState.IsValid)
            {
                var car = new Car()
                {
                    EnginePower = pCarMember.EnginePower,
                    EngingeCapacity = pCarMember.EngingeCapacity,
                    Make = pCarMember.Make,
                    Model = pCarMember.Model,
                    RegPlate = pCarMember.RegPlate
                };

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                var carMember = new CarMember()
                {
                    CarCarId = car.CarId,
                    CarMemberId = pCarMember.MemberId
                };
                _context.CarMembers.Add(carMember);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(pCarMember);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Make,Model,EngingeCapacity,EnginePower,RegPlate")] Car car)
        {
            if (id != car.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.CarId))
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
            return View(car);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }
    }
}
