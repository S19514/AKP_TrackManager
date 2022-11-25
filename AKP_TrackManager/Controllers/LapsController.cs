using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AKP_TrackManager.Controllers
{
    public class LapsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public LapsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? page)
        {

            var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date = t.Date.ToString("dd.MM.yyyy") }).ToList();
            ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date");
            if (HttpContext.User.IsInRole("Admin")) // list all for Admin-privileged user
            {
                List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
                var laps = await _context.Laps.Include(l => l.TrainingTraining).ToListAsync();
                foreach (var lap in laps)
                {
                    var memberCarOnLap = await _context.MemberCarOnLaps.Where(m => m.LapLapId == lap.LapId).FirstOrDefaultAsync();
                    var member = await _context.Members.Where(m => m.MemberId == memberCarOnLap.MemberMemberId).FirstOrDefaultAsync();
                    var car = await _context.Cars.Where(c => c.CarId == memberCarOnLap.CarCarId).FirstOrDefaultAsync();
                    memberCarOnLapsDto.Add(new MemberCarOnLapDto
                    {
                        AbsoluteTime = lap.AbsoluteTime,
                        CarId = car.CarId,
                        DateOfBirth = member.DateOfBirth,
                        TrainingTrainingId = lap.TrainingTrainingId,
                        TrainingDate = lap.TrainingTraining.Date,
                        EmailAddress = member.EmailAddress,
                        LapId = lap.LapId,
                        Name = member.Name,
                        Surname = member.Surname,
                        RegPlate = car.RegPlate,
                        MemberId = member.MemberId,
                        PenaltyTime = lap.PenaltyTime,
                        MeasuredTime = lap.MeasuredTime,

                    });
                    //todo ogarnąć dlaczego data treningu jest taka zwalona
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);

                return View(PagedList);
            }
            else
            {
                List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
                var member = await _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefaultAsync();
                var membersCarsOnLaps = _context.MemberCarOnLaps.Where(m=>m.MemberMemberId == member.MemberId).ToList();
                foreach(var mcol in membersCarsOnLaps)
                {
                    var lap = await _context.Laps.Include(t=>t.TrainingTraining).Where(m=>m.LapId == mcol.LapLapId).FirstOrDefaultAsync();
                    var car = await _context.Cars.FindAsync(mcol.CarCarId);
                    memberCarOnLapsDto.Add(new MemberCarOnLapDto
                    {
                        AbsoluteTime = lap.AbsoluteTime,
                        CarId = car.CarId,
                        DateOfBirth = member.DateOfBirth,
                        TrainingTrainingId = lap.TrainingTrainingId,
                        EmailAddress = member.EmailAddress,
                        LapId = lap.LapId,
                        Name = member.Name,
                        Surname = member.Surname,
                        RegPlate = car.RegPlate,
                        MemberId = member.MemberId,
                        PenaltyTime = lap.PenaltyTime,
                        MeasuredTime = lap.MeasuredTime,
                        TrainingDate = lap.TrainingTraining.Date
                    });
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t=>t.TrainingDate), pageNumber, pageSize);

                return View(PagedList);
            }
        }

        [System.Web.Http.Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
                var member = await _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefaultAsync();
                var membersCarsOnLaps = _context.MemberCarOnLaps.Where(m => m.MemberMemberId == member.MemberId).ToList();
                foreach (var mcol in membersCarsOnLaps)
                {
                    var lap = await _context.Laps.Include(t => t.TrainingTraining).Where(m => m.LapId == mcol.LapLapId).FirstOrDefaultAsync();
                    var car = await _context.Cars.FindAsync(mcol.CarCarId);
                    memberCarOnLapsDto.Add(new MemberCarOnLapDto
                    {
                        AbsoluteTime = lap.AbsoluteTime,
                        CarId = car.CarId,
                        DateOfBirth = member.DateOfBirth,
                        TrainingTrainingId = lap.TrainingTrainingId,
                        EmailAddress = member.EmailAddress,
                        LapId = lap.LapId,
                        Name = member.Name,
                        Surname = member.Surname,
                        RegPlate = car.RegPlate,
                        MemberId = member.MemberId,
                        PenaltyTime = lap.PenaltyTime,
                        MeasuredTime = lap.MeasuredTime,
                        TrainingDate = lap.TrainingTraining.Date
                    });
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);
                return View("IndexAdmin", PagedList);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> IndexByTrainingId(int? id, int? page)
        {
           
                List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
                var member = await _context.Members.Where(m => m.EmailAddress == HttpContext.User.Identity.Name).FirstOrDefaultAsync();
                var membersCarsOnLaps = _context.MemberCarOnLaps.Where(m => m.MemberMemberId == member.MemberId && m.LapLap.TrainingTrainingId == id).ToList();
                foreach (var mcol in membersCarsOnLaps)
                {
                    var lap = await _context.Laps.Include(t => t.TrainingTraining).Where(m => m.LapId == mcol.LapLapId).FirstOrDefaultAsync();
                    var car = await _context.Cars.FindAsync(mcol.CarCarId);
                    memberCarOnLapsDto.Add(new MemberCarOnLapDto
                    {
                        AbsoluteTime = lap.AbsoluteTime,
                        CarId = car.CarId,
                        DateOfBirth = member.DateOfBirth,
                        TrainingTrainingId = lap.TrainingTrainingId,
                        EmailAddress = member.EmailAddress,
                        LapId = lap.LapId,
                        Name = member.Name,
                        Surname = member.Surname,
                        RegPlate = car.RegPlate,
                        MemberId = member.MemberId,
                        PenaltyTime = lap.PenaltyTime,
                        MeasuredTime = lap.MeasuredTime,
                        TrainingDate = lap.TrainingTraining.Date
                    });
                }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);

            return View("IndexByTrainingId", PagedList);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lap = await _context.Laps
                .Include(l => l.TrainingTraining)
                .FirstOrDefaultAsync(m => m.LapId == id);
            if (lap == null)
            {
                return NotFound();
            }
            var memberCarOnLap = await _context.MemberCarOnLaps.Where(m => m.LapLapId == lap.LapId).FirstOrDefaultAsync();
            if (memberCarOnLap == null)
            {
                return NotFound();
            }
            var car = await _context.Cars.FindAsync(memberCarOnLap.CarCarId);
            if(car == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(memberCarOnLap.MemberMemberId);
            if(member == null)
            {
                return NotFound();
            }

            var memberCarOnLapDto = new MemberCarOnLapDto
            {
                AbsoluteTime = lap.AbsoluteTime,
                CarId = car.CarId,
                DateOfBirth = member.DateOfBirth,
                TrainingTrainingId = lap.TrainingTrainingId,
                EmailAddress = member.EmailAddress,
                LapId = lap.LapId,
                Name = member.Name,
                Surname = member.Surname,
                RegPlate = car.RegPlate,
                MemberId = member.MemberId,
                PenaltyTime = lap.PenaltyTime,
                MeasuredTime = lap.MeasuredTime,
                TrainingDate = lap.TrainingTraining.Date,
                Model = car.Model,
                Make = car.Make,
                TrainingLocationString = await _context.Locations.Where(l => l.LocationId == lap.TrainingTraining.LocationLocationId).Select(l => l.FriendlyName).FirstOrDefaultAsync()
            };
            return View(memberCarOnLapDto);
        }

        public IActionResult Create()
        {
            var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date=t.Date.ToString("dd.MM.yyyy") }).ToList();
            ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date");
            ViewData["EmailAddress"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            ViewData["RegPlate"] = new SelectList(_context.Cars, "CarId", "RegPlate");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LapId,MeasuredTime,PenaltyTime,AbsoluteTime,TrainingTrainingId,MemberId,CarId")] MemberCarOnLapDto pMemberLapOnCar)
        {
            if (ModelState.IsValid)
            {
                var lap = new Lap()
                {
                    AbsoluteTime = pMemberLapOnCar.AbsoluteTime,
                    MeasuredTime = pMemberLapOnCar.MeasuredTime,
                    PenaltyTime = pMemberLapOnCar.PenaltyTime,
                    TrainingTrainingId = pMemberLapOnCar.TrainingTrainingId
                };
                _context.Laps.Add(lap);
                await _context.SaveChangesAsync();

                MemberCarOnLap memberCarOnLap = new MemberCarOnLap()
                {
                    CarCarId = pMemberLapOnCar.CarId,
                    LapLapId = lap.LapId,
                    MemberMemberId = pMemberLapOnCar.MemberId,
                };
                _context.MemberCarOnLaps.Add(memberCarOnLap);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", pMemberLapOnCar.TrainingTrainingId);
            var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date = t.Date.ToString("dd.MM.yyyy") }).ToList();
            ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date",pMemberLapOnCar.TrainingTrainingId);
            ViewData["EmailAddress"] = new SelectList(_context.Members, "MemberId", "EmailAddress",pMemberLapOnCar.MemberId);
            ViewData["RegPlate"] = new SelectList(_context.Cars, "CarId", "RegPlate",pMemberLapOnCar.CarId);

            return View(pMemberLapOnCar);
        }

        #region not used
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var lap = await _context.Laps.Include(t=>t.TrainingTraining).Where(l=>l.LapId == id).FirstOrDefaultAsync();
        //    if (lap == null)
        //    {
        //        return NotFound();
        //    }
        //    var memberCarOnLap = await _context.MemberCarOnLaps.Where(m => m.LapLapId == lap.LapId).FirstOrDefaultAsync();
        //    if(memberCarOnLap== null)
        //    {
        //        return NotFound();
        //    }
        //    var member = await _context.Members.FindAsync(memberCarOnLap.MemberMemberId);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }
        //    var car = await _context.Cars.FindAsync(memberCarOnLap.CarCarId);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }
        //    var memberOnCarLapDto = new MemberCarOnLapDto
        //    {
        //        AbsoluteTime = lap.AbsoluteTime,
        //        CarId = car.CarId,
        //        DateOfBirth = member.DateOfBirth,
        //        TrainingTrainingId = lap.TrainingTrainingId,
        //        TrainingDate = lap.TrainingTraining.Date,
        //        EmailAddress = member.EmailAddress,
        //        LapId = lap.LapId,
        //        Name = member.Name,
        //        Surname = member.Surname,
        //        RegPlate = car.RegPlate,
        //        MemberId = member.MemberId,
        //        PenaltyTime = lap.PenaltyTime,
        //        MeasuredTime = lap.MeasuredTime,
        //    };

        //    //ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
        //    var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date = t.Date.ToString("dd.MM.yyyy") }).ToList();
        //    ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date", lap.TrainingTrainingId);
        //    ViewData["EmailAddress"] = new SelectList(_context.Members, "MemberId", "EmailAddress", member.MemberId);
        //    ViewData["RegPlate"] = new SelectList(_context.Cars, "CarId", "RegPlate", car.CarId);
        //    return View(memberOnCarLapDto);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("LapId,MeasuredTime,PenaltyTime,AbsoluteTime,TrainingTrainingId")] MemberCarOnLapDto pMemberCarOnLapDto)
        //{
        //    if (id != pMemberCarOnLapDto.LapId)
        //    {
        //        return NotFound();
        //    }
        //    var Training = await _context.training.FindAsync(pMemberCarOnLapDto.TrainingTrainingId);
        //    if (Training == null)
        //    {
        //        return NotFound();
        //    }

        //    var lap = await _context.Laps.FindAsync(id);
        //    var member = await _context.Members.FindAsync(pMemberCarOnLapDto.MemberId);
        //    if(member == null)
        //    {
        //        return NotFound();
        //    }
        //    var car = await _context.Cars.FindAsync(pMemberCarOnLapDto.CarId);
        //    if(car == null)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            lap.MeasuredTime = pMemberCarOnLapDto.MeasuredTime;
        //            lap.AbsoluteTime = pMemberCarOnLapDto.AbsoluteTime;
        //            lap.PenaltyTime = pMemberCarOnLapDto.PenaltyTime;
        //            lap.TrainingTrainingId = pMemberCarOnLapDto.TrainingTrainingId;

        //            _context.Laps.Update(lap);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!LapExists(pMemberCarOnLapDto.LapId))
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
        //    var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date = t.Date.ToString("dd.MM.yyyy") }).ToList();
        //    ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date", lap.TrainingTrainingId);
        //    ViewData["EmailAddress"] = new SelectList(_context.Members, "MemberId", "EmailAddress", member.MemberId);
        //    ViewData["RegPlate"] = new SelectList(_context.Cars, "CarId", "RegPlate", car.CarId);            
        //    return View(pMemberCarOnLapDto);
        //}
        #endregion

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lap = await _context.Laps
                .Include(l => l.TrainingTraining)
                .FirstOrDefaultAsync(m => m.LapId == id);
            if (lap == null)
            {
                return NotFound();
            }
            var memberCarOnLap = await _context.MemberCarOnLaps.Where(m => m.LapLapId == lap.LapId).FirstOrDefaultAsync();
            if (memberCarOnLap == null)
            {
                return NotFound();
            }
            var car = await _context.Cars.FindAsync(memberCarOnLap.CarCarId);
            if (car == null)
            {
                return NotFound();
            }
            var member = await _context.Members.FindAsync(memberCarOnLap.MemberMemberId);
            if (member == null)
            {
                return NotFound();
            }

            var memberCarOnLapDto = new MemberCarOnLapDto
            {
                AbsoluteTime = lap.AbsoluteTime,
                CarId = car.CarId,
                DateOfBirth = member.DateOfBirth,
                TrainingTrainingId = lap.TrainingTrainingId,
                EmailAddress = member.EmailAddress,
                LapId = lap.LapId,
                Name = member.Name,
                Surname = member.Surname,
                RegPlate = car.RegPlate,
                MemberId = member.MemberId,
                PenaltyTime = lap.PenaltyTime,
                MeasuredTime = lap.MeasuredTime,
                TrainingDate = lap.TrainingTraining.Date,
                Model = car.Model,
                Make = car.Make,
                TrainingLocationString = await _context.Locations.Where(l => l.LocationId == lap.TrainingTraining.LocationLocationId).Select(l => l.FriendlyName).FirstOrDefaultAsync()
            };
            return View(memberCarOnLapDto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lap = await _context.Laps.FindAsync(id);
            var memberCarOnLaps = await _context.MemberCarOnLaps.Where(m => m.LapLapId == id).FirstOrDefaultAsync();
            _context.MemberCarOnLaps.Remove(memberCarOnLaps);
            _context.Laps.Remove(lap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LapExists(int id)
        {
            return _context.Laps.Any(e => e.LapId == id);
        }
    }
}
