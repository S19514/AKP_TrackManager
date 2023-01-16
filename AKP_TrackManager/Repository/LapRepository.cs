using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKP_TrackManager.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;

namespace AKP_TrackManager.Repository
{
    public class LapRepository : ILapRepository
    {
        private AKP_TrackManager_devContext _context;
        public LapRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<MemberCarOnLapDto> Create(MemberCarOnLapDto pMemberLapOnCar)
        {
            var lap = new Lap()
            {
                AbsoluteTime = pMemberLapOnCar.AbsoluteTime,
                MeasuredTime = pMemberLapOnCar.MeasuredTime,
                PenaltyTime = pMemberLapOnCar.PenaltyTime,
                TrainingTrainingId = pMemberLapOnCar.TrainingTrainingId
            };
            try
            {
                _context.Laps.Add(lap);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                string message = ex.ToString();
                return null;
            }

            MemberCarOnLap memberCarOnLap = new MemberCarOnLap()
            {
                CarCarId = pMemberLapOnCar.CarId,
                LapLapId = lap.LapId,
                MemberMemberId = pMemberLapOnCar.MemberId,
            };
            try
            {
                _context.MemberCarOnLaps.Add(memberCarOnLap);
                await _context.SaveChangesAsync();
                return pMemberLapOnCar;
            }
            catch(Exception ex) 
            {
                string message = ex.ToString();
                _context.Laps.Remove(lap);
                await _context.SaveChangesAsync();
                return null;
            }
        }

        public async Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin)
        {
            var lap = await _context.Laps.FindAsync(id);
            var memberCarOnLaps = await _context.MemberCarOnLaps.Where(m => m.LapLapId == id).FirstOrDefaultAsync();            
            if (memberCarOnLaps != null)
            {
                var member = await _context.Members.FindAsync(memberCarOnLaps.MemberMemberId);
                if (member != null && (member.EmailAddress == contextUserName || isAdmin))
                {
                    _context.MemberCarOnLaps.Remove(memberCarOnLaps);
                }
                else 
                    return false;
            }
            if (lap != null)
            {
                _context.Laps.Remove(lap);
            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                string message = ex.ToString();
                return false;
            }
        }

        public async Task<MemberCarOnLapDto> Details(int? id, string contextUserName, bool isAdmin)
        {
            var lap = await _context.Laps
              .Include(l => l.TrainingTraining)
              .FirstOrDefaultAsync(m => m.LapId == id);
            if (lap == null)
            {
                return null;
            }
            var memberCarOnLap = await _context.MemberCarOnLaps.Where(m => m.LapLapId == lap.LapId).FirstOrDefaultAsync();
            if (memberCarOnLap == null)
            {
                return null;
            }
            var car = await _context.Cars.FindAsync(memberCarOnLap.CarCarId);
            if (car == null)
            {
                return null;
            }
            var member = await _context.Members.FindAsync(memberCarOnLap.MemberMemberId);
            if (member == null)
            {
                return null;
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
            if (contextUserName == member.EmailAddress || isAdmin)
            {
                return memberCarOnLapDto;
            }
            else
                return null;
        }

        public SelectList GetMembersSelectList()
        {
            throw new NotImplementedException();
        }

        public SelectList GetRegPlatesSelectList()
        {
            throw new NotImplementedException();
        }

        public SelectList GetSelectedMemberSelectList(int memberId)
        {
            throw new NotImplementedException();
        }

        public SelectList GetSelectedRegPlatesSelectList(int carId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MemberCarOnLapDto>> Index(int? page, string contextUserName, bool isAdmin, string searchCar, string searchMember, DateTime? searchDate)
        {
            if (isAdmin) // list all for Admin-privileged user
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
                }
                if(!string.IsNullOrEmpty(searchCar))
                {
                    memberCarOnLapsDto = memberCarOnLapsDto.Where(c => c.RegPlate!.Contains(searchCar)).ToList();
                }
                if (!string.IsNullOrEmpty(searchMember))
                {
                    memberCarOnLapsDto = memberCarOnLapsDto.Where(c => (c.Name + " " + c.Surname)!.Contains(searchMember)).ToList();
                }
                if (searchDate != null)
                {
                    memberCarOnLapsDto = memberCarOnLapsDto.Where(t=>t.TrainingDate!.Equals(searchDate)).ToList();

                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new X.PagedList.PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);

                return PagedList;
            }
            else
            {
                List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
                var member = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
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
                if (!string.IsNullOrEmpty(searchCar))
                {
                    memberCarOnLapsDto = memberCarOnLapsDto.Where(c => c.RegPlate!.Contains(searchCar)).ToList();
                }
                if (!string.IsNullOrEmpty(searchMember))
                {
                    memberCarOnLapsDto = memberCarOnLapsDto.Where(c => (c.Name + " " + c.Surname)!.Contains(searchMember)).ToList();
                }
                if (searchDate != null)
                {
                    memberCarOnLapsDto = memberCarOnLapsDto.Where(t => t.TrainingDate!.Equals(searchDate)).ToList();

                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new X.PagedList.PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);

                return PagedList;
            }
        }

        public async Task<IEnumerable<MemberCarOnLapDto>> IndexByTrainingId(int? id, int? page, string contextUserName)
        {
             List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
                var member = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
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
            X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new X.PagedList.PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);
            if (contextUserName == member.EmailAddress)
            {
                return PagedList;
            }
            else
                return null;
        }

        public async Task<IEnumerable<MemberCarOnLapDto>> IndexFilterAdmin(int? page, string contextUserName, string searchCar, DateTime? searchDate)
        {
            List<MemberCarOnLapDto> memberCarOnLapsDto = new List<MemberCarOnLapDto>();
            var member = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
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
            if (!string.IsNullOrEmpty(searchCar))
            {
                memberCarOnLapsDto = memberCarOnLapsDto.Where(c => c.RegPlate!.Contains(searchCar)).ToList();
            }
            if (searchDate != null)
            {
                memberCarOnLapsDto = memberCarOnLapsDto.Where(t => t.TrainingDate!.Equals(searchDate)).ToList();

            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<MemberCarOnLapDto> PagedList = new X.PagedList.PagedList<MemberCarOnLapDto>(memberCarOnLapsDto.OrderByDescending(t => t.TrainingDate), pageNumber, pageSize);
            return PagedList;
        }
    }
}
