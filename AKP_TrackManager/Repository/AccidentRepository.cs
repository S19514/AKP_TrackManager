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
using System.Data.Entity;


namespace AKP_TrackManager.Repository
{
    public class AccidentRepository : IAccidentRepository
    {
        private AKP_TrackManager_devContext _context;
        public AccidentRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public bool AccidentExists(int id)
        {
            return _context.Accidents.Any(e => e.AccidentId == id); 
        }

        public async Task<AccidentCarMemberDto> Create(AccidentCarMemberDto accident)
        {
            var postAccident = new Accident()
            {
                AccidentDate = accident.AccidentDate,
                AnyoneInjured = accident.AnyoneInjured,
                Severity = accident.Severity
            };
            _context.Accidents.Add(postAccident);
            await _context.SaveChangesAsync();

            var carAccidentByMember = new CarAccidentByMember()
            {
                AccidentAccidentId = postAccident.AccidentId,
                CarCarId = accident.CarId,
                MemberMemberId = accident.MemberId
            };
            _context.CarAccidentByMembers.Add(carAccidentByMember);
            try
            {
                await _context.SaveChangesAsync();
                return null;
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return accident;
            }
        }

        public async Task<AccidentCarMemberDto> Delete(int? id, string contextUserName, bool isAdmin)
        {

            var accident = _context.Accidents.FirstOrDefault(m => m.AccidentId == id);
            if(accident == null)
            {
                return null;
            }
            var carAccidentByMember = _context.CarAccidentByMembers.FirstOrDefault(m => m.AccidentAccidentId == id);
            if(carAccidentByMember == null)
            {
                return null;
            }
            var car =  _context.Cars.FirstOrDefault(m => m.CarId == carAccidentByMember.CarCarId);
            if(car == null)
            {
                return null;
            }
            var member = _context.Members.FirstOrDefault(m => m.MemberId == carAccidentByMember.MemberMemberId);
            if (member == null || (member.EmailAddress != contextUserName && !isAdmin))
            {
                return null;
            }

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


            if (accidentCarMemberDto == null)
            {
                return null;
            }
            return accidentCarMemberDto;
        }

        public async Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin)
        {
            var accident = _context.Accidents.Find(id);
            if(accident == null) 
            { 
                return false; 
            }
            var carAccidentByMember = _context.CarAccidentByMembers.Where(cabm => cabm.AccidentAccidentId == accident.AccidentId)
                                                                            .FirstOrDefault();
            if(carAccidentByMember == null)
            { 
                return false; 
            }
            _context.CarAccidentByMembers.Remove(carAccidentByMember);
            _context.Accidents.Remove(accident);
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }

        public async Task<AccidentCarMemberDto> Details(int? id, string contextUserName, bool isAdmin)
        {
            if (id == null)
            {
                return null;
            }

            var accident = _context.Accidents.FirstOrDefault(m => m.AccidentId == id);
            if (accident == null)
            {
                return null;
            }
            var carAccidentByMember = _context.CarAccidentByMembers.FirstOrDefault(m => m.AccidentAccidentId == id);
            if (carAccidentByMember == null)
            {
                return null;
            }
            var car = _context.Cars.FirstOrDefault(m => m.CarId == carAccidentByMember.CarCarId);
            if (car == null)
            {
                return null;
            }
            var member = _context.Members.FirstOrDefault(m => m.MemberId == carAccidentByMember.MemberMemberId);
            if (member == null || (member.EmailAddress != contextUserName && !isAdmin))
            {
                return null;
            }

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

            return accidentCarMemberDto;
        }

        public async Task<AccidentCarMemberDto> Edit(int? id, string contextUserName, bool isAdmin)
        {
            var accident =  _context.Accidents.Find(id);
            if(accident == null)
            {
                return null;
            }
            var carAccidentByMember =  _context.CarAccidentByMembers.Where(cabm => cabm.AccidentAccidentId == id).FirstOrDefault();
            if (carAccidentByMember == null)
            { 
                return null; 
            }
            var car = _context.Cars.Where(car => car.CarId == carAccidentByMember.CarCarId).FirstOrDefault();
            if(car == null) 
            {
                return null;
            }
            var member = _context.Members.Where(mem => mem.MemberId == carAccidentByMember.MemberMemberId).FirstOrDefault();
            if (member == null || (member.EmailAddress != contextUserName && !isAdmin))
            {
                return null;
            }

            AccidentCarMemberDto acmd = new AccidentCarMemberDto()
            {
                AccidentDate = accident.AccidentDate,
                AccidentId = accident.AccidentId,
                AnyoneInjured = accident.AnyoneInjured,
                CarId = carAccidentByMember.CarCarId,
                Severity = accident.Severity,
                RegPlate = car.RegPlate,
                MemberId = carAccidentByMember.MemberMemberId,
                EmailAddress = member.EmailAddress
            };      
            return acmd;
        }

        public async Task<AccidentCarMemberDto> Edit(int id, AccidentCarMemberDto accident, string contextUserName, bool isAdmin)
        {
            try
            {
                var carAccidentByMember = await _context.CarAccidentByMembers.Where(c => c.AccidentAccidentId == accident.AccidentId).FirstOrDefaultAsync();
                var accidentStored = await _context.Accidents.Where(a => a.AccidentId == accident.AccidentId).FirstOrDefaultAsync();
                if (carAccidentByMember == null || accidentStored == null)
                {
                    return null;
                }
                carAccidentByMember.MemberMemberId = accident.MemberId;
                carAccidentByMember.CarCarId = accident.CarId;
                accidentStored.AccidentDate = accident.AccidentDate;
                accidentStored.Severity = accident.Severity;
                accidentStored.AnyoneInjured = accident.AnyoneInjured;

                _context.Accidents.Update(accidentStored);
                _context.CarAccidentByMembers.Update(carAccidentByMember);
                await _context.SaveChangesAsync();
                return accident;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccidentExists(accident.AccidentId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            };
        }

        public SelectList GetMembersSelectList()
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress");
        }

        public SelectList GetRegPlatesSelectList()
        {
            return new SelectList(_context.Cars, "CarId", "RegPlate");
        }

        public SelectList GetSelectedMemberSelectList(int memberId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Accident>> Index(int? page, string contextUserName, bool isAdmin)
        {
            if (isAdmin)
            {
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Accident> PagedList = new X.PagedList.PagedList<Accident>(_context.Accidents.ToList(), pageNumber, pageSize);
                return PagedList;
            }
            else
            {
                List<Accident> accidentList = new List<Accident>();
                var member = _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefault();
                    var carAccidentByMember =  _context.CarAccidentByMembers.Where(c => c.MemberMemberId == member.MemberId).ToList();

                foreach (var carAccident in carAccidentByMember)
                {
                    accidentList.Add(_context.Accidents.Where(a => a.AccidentId == carAccident.AccidentAccidentId).FirstOrDefault());
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Accident> PagedList = new X.PagedList.PagedList<Accident>(accidentList, pageNumber, pageSize);

                return PagedList;
            }
        }

        public async Task<IEnumerable<Accident>> IndexFilterAdmin(int? page, string contextUserName)
        {
            List<Accident> accidentList = new List<Accident>();
            var member = _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefault();
            var carAccidentByMember =  _context.CarAccidentByMembers.Where(c => c.MemberMemberId == member.MemberId).ToList();

            foreach (var carAccident in carAccidentByMember)
            {
                accidentList.Add( _context.Accidents.Where(a => a.AccidentId == carAccident.AccidentAccidentId).FirstOrDefault());
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<Accident> PagedList = new X.PagedList.PagedList<Accident>(accidentList, pageNumber, pageSize);
            return PagedList;

        }
    }
}
