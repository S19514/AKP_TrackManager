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
    public class CarRepository : ICarRepository
    {
        private AKP_TrackManager_devContext _context;
        public CarRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.CarId == id);
        }

        public async Task<CarMemberDto> Create(CarMemberDto pCarMember)
        {
            var car = new Car()
            {
                EnginePower = pCarMember.EnginePower,
                EngingeCapacity = pCarMember.EngingeCapacity,
                Make = pCarMember.Make.Trim(),
                Model = pCarMember.Model.Trim(),
                RegPlate = pCarMember.RegPlate.Replace(" ", "")
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            var carMember = new CarMember()
            {
                CarCarId = car.CarId,
                MemberMemberId = pCarMember.MemberId
            };
            _context.CarMembers.Add(carMember);
            try
            {
                await _context.SaveChangesAsync();
                return null;
            }
            catch(Exception ex)
            {
                string exceptionMessage = ex.Message;
                return pCarMember;
            }
        }

        public async Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin)
        {
            var car = await _context.Cars.FindAsync(id);
            var carMember = await _context.CarMembers.Where(cm => cm.CarCarId == id).FirstOrDefaultAsync();
            var carAccidents = await _context.CarAccidentByMembers.Where(ca => ca.CarCarId == id).ToListAsync();
            var carLaps = await _context.MemberCarOnLaps.Where(cl => cl.CarCarId == id).ToListAsync();

            #region privilege check
            var memberCheck = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
            if (memberCheck == null)
            {
                return false;
            }

            if (carMember.MemberMemberId != memberCheck.MemberId && !isAdmin)
                return false;
            #endregion
            foreach (var accident in carAccidents)
            {
                _context.CarAccidentByMembers.Remove(accident);
                _context.Accidents.Remove(_context.Accidents.Find(accident.AccidentAccidentId));
            }
            foreach (var lap in carLaps)
            {
                _context.MemberCarOnLaps.Remove(lap);
                _context.Laps.Remove(_context.Laps.Find(lap.LapLapId));
            }
            _context.CarMembers.Remove(carMember);
            _context.Cars.Remove(car);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                return false;
            }
        }

        public async Task<CarMemberDto> Details(int? id, string contextUserName, bool isAdmin)
        {
            if (id == null)
            {
                return null;
            }
            var car = await _context.Cars.Where(c => c.CarId == id).Include(c => c.CarAccidentByMembers).FirstOrDefaultAsync();
            if (car == null)
            {
                return null;
            }
            var carMember = await _context.CarMembers.Where(c => c.CarCarId == id).FirstOrDefaultAsync();
            if (carMember == null)
            {
                return null;
            }
            #region privilege check
            var memberCheck = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
            if (memberCheck == null)
            {
                return null;
            }

            if (carMember.MemberMemberId != memberCheck.MemberId && !isAdmin)
                return null;
            #endregion

            foreach (var acc in car.CarAccidentByMembers)
            {
                acc.AccidentAccident = await _context.Accidents.FindAsync(acc.AccidentAccidentId);
            };

            var member = await _context.Members.FindAsync(carMember.MemberMemberId);
            if (member == null)
            {
                return null;
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
                RegPlate = car.RegPlate.Replace(" ", ""),
                CarAccidentByMembers = car.CarAccidentByMembers
            };
            return carMemberDto;
        }

        public async Task<CarMemberDto> Edit(int id, CarMemberDto pCarMemberDto, string contextUserName, bool isAdmin)
        {
            try
            {
                var car = await _context.Cars.Where(c => c.CarId == id).FirstOrDefaultAsync();
                car.EnginePower = pCarMemberDto.EnginePower;
                car.RegPlate = pCarMemberDto.RegPlate.Replace(" ", "");
                car.Make = pCarMemberDto.Make.Trim();
                car.Model = pCarMemberDto.Model.Trim();
                car.EngingeCapacity = pCarMemberDto.EngingeCapacity;
                _context.Cars.Update(car);

                var carMember = await _context.CarMembers.Where(cm => cm.CarCarId == pCarMemberDto.CarId).FirstOrDefaultAsync();
                carMember.MemberMemberId = pCarMemberDto.MemberId;
                _context.CarMembers.Update(carMember);

                await _context.SaveChangesAsync();
                return pCarMemberDto;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(pCarMemberDto.CarId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<CarMemberDto> Edit(int? id, string contextUserName, bool isAdmin)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return null;
            }
            var carMember = await _context.CarMembers.Where(c => c.CarCarId == id).FirstOrDefaultAsync();
            if (carMember == null)
            {
                return null;
            }
            #region privilege check
            var memberCheck = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
            if (memberCheck == null)
            {
                return null;
            }

            if (carMember.MemberMemberId != memberCheck.MemberId && !isAdmin)
                return null;
            #endregion

            var member = await _context.Members.FindAsync(carMember.MemberMemberId);
            if (member == null)
            {
                return null;
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
            return carMemberDto;
        }

        public SelectList GetMembersSelectList()
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress");
        }

        public SelectList GetSelectedMemberSelectList(int memberId)
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress", memberId);
        }

        public async Task<IEnumerable<CarMemberDto>> Index(int? page, string contextUserName, bool isAdmin, string searchString)
        {
            if (isAdmin) // list all for Admin-privileged user
            {
                List<CarMemberDto> carMemberDtos = new List<CarMemberDto>();
                var cars = await _context.Cars.ToListAsync();
                foreach (var car in cars)
                {
                    var singleCarMember = await _context.CarMembers.Where(c => c.CarCarId == car.CarId).FirstOrDefaultAsync();
                    if (singleCarMember == null)
                        return carMemberDtos;

                    var singleMember = await _context.Members.Where(m => m.MemberId == singleCarMember.MemberMemberId).FirstOrDefaultAsync();
                    if (singleMember == null)
                        return carMemberDtos;

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
                if (!String.IsNullOrEmpty(searchString))
                {
                    carMemberDtos = carMemberDtos.Where(c => c.RegPlate!.Contains(searchString)).ToList();
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<CarMemberDto> PagedList = new X.PagedList.PagedList<CarMemberDto>(carMemberDtos, pageNumber, pageSize);
                return PagedList;
            }
            else // list only belonging cars to User-privileged user
            {
                List<CarMemberDto> carMemberDtos = new List<CarMemberDto>();
                List<Car> cars = new List<Car>();
                var member = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
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
                if (!String.IsNullOrEmpty(searchString))
                {
                    carMemberDtos = carMemberDtos.Where(c => c.RegPlate!.Contains(searchString)).ToList();
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<CarMemberDto> PagedList = new X.PagedList.PagedList<CarMemberDto>(carMemberDtos, pageNumber, pageSize);
                return PagedList;
            }
        }

        public async Task<IEnumerable<CarMemberDto>> IndexFilterAdmin(int? page, string contextUserName)
        {
            List<CarMemberDto> carMemberDtos = new List<CarMemberDto>();
            List<Car> cars = new List<Car>();
            var member = _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefault();
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
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<CarMemberDto> PagedList = new X.PagedList.PagedList<CarMemberDto>(carMemberDtos, pageNumber, pageSize);
            return PagedList;
        }
    }
}
