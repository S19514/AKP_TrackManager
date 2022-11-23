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
    public class LapsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public LapsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
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
                        EmailAddress = member.EmailAddress,
                        LapId = lap.LapId,
                        Name = member.Name,
                        Surname = member.Surname,
                        RegPlate = car.RegPlate,
                        MemberId = member.MemberId,
                        PenaltyTime = lap.PenaltyTime,
                        MeasuredTime = lap.MeasuredTime,

                    });
                }
                return View(memberCarOnLapsDto);
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
                return View(memberCarOnLapsDto);
            }
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

            return View(lap);
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
        public async Task<IActionResult> Create([Bind("LapId,MeasuredTime,PenaltyTime,AbsoluteTime,TrainingTrainingId,MemberId,CarId")] Lap lap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
            return View(lap);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lap = await _context.Laps.FindAsync(id);
            if (lap == null)
            {
                return NotFound();
            }
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
            return View(lap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LapId,MeasuredTime,PenaltyTime,AbsoluteTime,TrainingTrainingId")] Lap lap)
        {
            if (id != lap.LapId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LapExists(lap.LapId))
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
            ViewData["TrainingTrainingId"] = new SelectList(_context.training, "TrainingId", "TrainingId", lap.TrainingTrainingId);
            return View(lap);
        }

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

            return View(lap);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lap = await _context.Laps.FindAsync(id);
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
