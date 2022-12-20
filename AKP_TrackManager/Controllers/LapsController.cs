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
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    [Authorize]
    public class LapsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private ILapRepository _lapRepository;
        public LapsController(AKP_TrackManager_devContext context, ILapRepository lapRepository)
        {
            _context = context;
            _lapRepository = lapRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {

            var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date = t.Date.ToString("dd.MM.yyyy") }).ToList();
            ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date");

            return View(await _lapRepository.Index(page, User.Identity.Name, User.IsInRole("Admin")));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
                return View("IndexAdmin", await _lapRepository.IndexFilterAdmin(page,User.Identity.Name));           
        }

        public async Task<IActionResult> IndexByTrainingId(int? id, int? page)
        {
            var PagedList = await _lapRepository.IndexByTrainingId(id, page,User.Identity.Name);           
               
            if(PagedList != null)
                return View("IndexByTrainingId", PagedList);
            
            else
               return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }          
            var memberCarOnLapDto = await _lapRepository.Details(id, User.Identity.Name,User.IsInRole("Admin"));
            if (memberCarOnLapDto != null)
            {
                return View(memberCarOnLapDto);
            }
            else
                return RedirectToAction(nameof(Index));
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
                var newMemberLapOnCar = await _lapRepository.Create(pMemberLapOnCar);
                if (newMemberLapOnCar != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            var trainingDates = _context.training.Select(t => new { TrainingId = t.TrainingId, Date = t.Date.ToString("dd.MM.yyyy") }).ToList();
            ViewData["Date"] = new SelectList(trainingDates, "TrainingId", "Date",pMemberLapOnCar.TrainingTrainingId);
            if (User.IsInRole("Admin"))
            {
                ViewData["EmailAddress"] = new SelectList(_context.Members, "MemberId", "EmailAddress", pMemberLapOnCar.MemberId);
            }
            else
            {
                ViewData["EmailAddress"] = new SelectList(_context.Members.Where(m=>m.EmailAddress == User.Identity.Name), "MemberId", "EmailAddress", pMemberLapOnCar.MemberId);
            }

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
            if (User.Identity.Name == member.EmailAddress)
            {
                return View(memberCarOnLapDto);
            }
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _lapRepository.DeleteConfirmed(id, User.Identity.Name, User.IsInRole("Admin"));
            return RedirectToAction(nameof(Index));
        }

        private bool LapExists(int id)
        {
            return _context.Laps.Any(e => e.LapId == id);
        }
    }
}
