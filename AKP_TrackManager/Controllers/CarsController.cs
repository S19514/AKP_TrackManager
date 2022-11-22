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
            if (HttpContext.User.IsInRole("Admin")) // list all for Admin-privileged user
            {
                List<CarMemberDto> carMemberDtos= new List<CarMemberDto>();
                var cars = await _context.Cars.ToListAsync();
                foreach(var car in cars)
                {
                    var singleCarMember =await _context.CarMembers.Where(c=>c.CarCarId== car.CarId).FirstOrDefaultAsync();
                    if (singleCarMember == null)
                        return View(carMemberDtos);

                    var singleMember = await _context.Members.Where(m=>m.MemberId == singleCarMember.MemberMemberId).FirstOrDefaultAsync();
                    if (singleMember == null)
                        return View(carMemberDtos);


                        carMemberDtos.Add(new CarMemberDto
                        {
                            CarId = car.CarId,
                            DateOfBirth = singleMember.DateOfBirth,
                            EmailAddress = singleMember.EmailAddress,
                            EnginePower = car.EnginePower,
                            EngingeCapacity = car.EngingeCapacity,
                            IsAscendant = singleMember.IsAscendant,
                            IsBlocked = singleMember.IsBlocked,
                            IsStudent = singleMember.IsStudent,
                            MemberId = singleMember.MemberId,
                            Name = singleMember.Name,
                            Make = car.Make,
                            Model = car.Model,
                            PhoneNumber = singleMember.PhoneNumber,
                            Surname = singleMember.Surname,
                            RegPlate = car.RegPlate

                        });
                    
                }

                return View(carMemberDtos);
            }
            else // list only belonging cars to User-privileged user
            {
                List<CarMemberDto> carMemberDtos= new List<CarMemberDto>();
                List<Car> cars = new List<Car>();
                var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();
                var carMembers =await _context.CarMembers.Where(cm=>cm.MemberMemberId == member.MemberId).ToListAsync();
                foreach(var carMember in carMembers)
                {
                    cars.Add(await _context.Cars.Where(c=>c.CarId == carMember.CarCarId).FirstOrDefaultAsync());
                }
                
                foreach(var car in cars)
                {
                    carMemberDtos.Add(new CarMemberDto
                    {
                        CarId = car.CarId,
                        DateOfBirth = member.DateOfBirth,
                        EmailAddress = member.EmailAddress,
                        EnginePower = car.EnginePower,
                        EngingeCapacity = car.EngingeCapacity,
                        IsAscendant = member.IsAscendant,
                        IsBlocked = member.IsBlocked,
                        IsStudent = member.IsStudent,
                        MemberId = member.MemberId,
                        Name = member.Name,
                        Make = car.Make,
                        Model = car.Model,
                        PhoneNumber = member.PhoneNumber,
                        Surname = member.Surname,
                        RegPlate = car.RegPlate

                    });
                }
                return View(carMemberDtos);
            }
        }

        [System.Web.Http.Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                List<CarMemberDto> carMemberDtos = new List<CarMemberDto>();
                List<Car> cars = new List<Car>();
                var member = _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefault();
                var carMembers = await _context.CarMembers.Where(cm => cm.MemberMemberId == member.MemberId).ToListAsync();
                foreach (var carMember in carMembers)
                {
                    cars.Add(await _context.Cars.Where(c => c.CarId == carMember.CarCarId).FirstOrDefaultAsync());
                }

                foreach (var car in cars)
                {
                    carMemberDtos.Add(new CarMemberDto
                    {
                        CarId = car.CarId,
                        DateOfBirth = member.DateOfBirth,
                        EmailAddress = member.EmailAddress.Replace(" ", ""),
                        EnginePower = car.EnginePower,
                        EngingeCapacity = car.EngingeCapacity,
                        IsAscendant = member.IsAscendant,
                        IsBlocked = member.IsBlocked,
                        IsStudent = member.IsStudent,
                        MemberId = member.MemberId,
                        Name = member.Name.Trim(),
                        Make = car.Make.Trim(),
                        Model = car.Model.Trim(),
                        PhoneNumber = member.PhoneNumber.Trim(),
                        Surname = member.Surname.Trim(),
                        RegPlate = car.RegPlate.Replace(" ", "")

                    }); ;
                }
                return View("Index", carMemberDtos);
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
                    Make = pCarMember.Make.Trim(),
                    Model = pCarMember.Model.Trim(),
                    RegPlate = pCarMember.RegPlate.Replace(" ","")
                };

                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                var carMember = new CarMember()
                {
                    CarCarId = car.CarId,
                    MemberMemberId = pCarMember.MemberId
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
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            var carMember = await _context.CarMembers.Where(c=>c.CarCarId == id).FirstOrDefaultAsync();
            if (carMember == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(carMember.MemberMemberId);
            if(member == null)
            {
                return NotFound();
            }
            var carMemberDto = new CarMemberDto
            {
                CarId = car.CarId,
                DateOfBirth = member.DateOfBirth,
                EmailAddress = member.EmailAddress.Replace(" ", ""),
                EnginePower = car.EnginePower,
                EngingeCapacity = car.EngingeCapacity,
                IsAscendant = member.IsAscendant,
                IsBlocked = member.IsBlocked,
                IsStudent = member.IsStudent,
                MemberId = member.MemberId,
                Name = member.Name.Trim(),
                Make = car.Make.Trim(),
                Model = car.Model.Trim(),
                PhoneNumber = member.PhoneNumber.Trim(),
                Surname = member.Surname.Trim(),
                RegPlate = car.RegPlate.Replace(" ", "")
            };

            return View(carMemberDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Make,Model,EngingeCapacity,EnginePower,RegPlate,MemberId")] CarMemberDto pCarMemberDto)
        {
            if (id != pCarMemberDto.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   var car = await _context.Cars.Where(c => c.CarId == id).FirstOrDefaultAsync();
                    car.EnginePower = pCarMemberDto.EnginePower;
                    car.RegPlate = pCarMemberDto.RegPlate.Replace(" ","");
                    car.Make = pCarMemberDto.Make.Trim();
                    car.Model = pCarMemberDto.Model.Trim();
                    car.EngingeCapacity = pCarMemberDto.EngingeCapacity;
                    _context.Cars.Update(car);

                    var carMember = await _context.CarMembers.Where(cm=> cm.CarCarId == pCarMemberDto.CarId).FirstOrDefaultAsync();
                    carMember.MemberMemberId = pCarMemberDto.MemberId;
                    _context.CarMembers.Update(carMember);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(pCarMemberDto.CarId))
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
            return View(pCarMemberDto);
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
