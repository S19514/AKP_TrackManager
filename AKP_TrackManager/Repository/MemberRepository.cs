using AKP_TrackManager.Interfaces;
using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AKP_TrackManager.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private AKP_TrackManager_devContext _context;
        public MemberRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }
        public SelectList Create()
        {
            return new SelectList(_context.Roles, "RoleId", "RoleName");
        }

        public async Task<MemberCreateDto> Create(MemberCreateDto member)
        {
            if (MemberMailChecker(member.Member.EmailAddress))
            {
                member.ModelErrorString = "Email is taken";
                member.ModelErrorKey = "Email exists";
                member.ViewData = GetRolesSelectedListItem(member.Member.RoleRoleId);
                return member;
            }
            _context.Members.Add(member.Member);
            await _context.SaveChangesAsync();

            ClubMembership membership = new ClubMembership()
            {
                FeeAmount = member.Member.IsStudent ? 50 : 100,
                JoinDate = DateTime.Now,
                MemberMemberId = member.Member.MemberId,

            };
            _context.ClubMemberships.Add(membership);
            await _context.SaveChangesAsync();
            member.Member = null;
            return  member;
        }

        public async Task<bool> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            var membership = await _context.ClubMemberships.Where(m => m.MemberMemberId == id).FirstOrDefaultAsync();
            if (membership != null)
            {
                var payments = await _context.Payments.Where(p => p.ClubMembershipMembershipId == membership.MembershipId).ToListAsync();

                foreach (var payment in payments)
                {
                    _context.Payments.Remove(payment);
                }
                _context.ClubMemberships.Remove(membership);
            }
            var carAccidentsByMember = await _context.CarAccidentByMembers.Where(cm => cm.MemberMemberId == id).ToListAsync();
            foreach (var accident in carAccidentsByMember)
            {
                _context.CarAccidentByMembers.Remove(accident);
                _context.Accidents.Remove(_context.Accidents.Find(accident.AccidentAccidentId));
            }
            var trainingAttendances = await _context.TrainingAttandances.Where(cm => cm.MemberMemberId == id).ToListAsync();
            foreach (var training in trainingAttendances)
            {
                _context.TrainingAttandances.Remove(training);
            }
            var memberCarOnLaps = await _context.MemberCarOnLaps.Where(mcol => mcol.MemberMemberId == id).ToListAsync();
            foreach (var lap in memberCarOnLaps)
            {
                _context.MemberCarOnLaps.Remove(lap);
                _context.Laps.Remove(_context.Laps.Find(lap.LapLapId));
            }
            var carsMember = await _context.CarMembers.Where(cm => cm.MemberMemberId == id).ToListAsync();
            foreach (var car in carsMember)
            {
                _context.CarMembers.Remove(car);
                _context.Cars.Remove(_context.Cars.Find(car.CarCarId));
            }

            _context.Members.Remove(member);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var exception = ex.Message;
                return false;
            }
            return true;
        }

        public async Task<Member> Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var member = await _context.Members
                .Include(m => m.RoleRole)
                .FirstOrDefaultAsync(m => m.MemberId == id);

            if (member == null)
            {
                return null;
            }

            return member;
        }

        public async Task<Member> Edit(int id, Member member)
        {
            try
            {              
                var membership = await _context.ClubMemberships.Where(m => m.MemberMemberId == member.MemberId).FirstOrDefaultAsync();
                if (membership == null)
                {
                    ClubMembership newMembership = new ClubMembership()
                    {
                        FeeAmount = member.IsStudent ? 50 : 100,
                        JoinDate = DateTime.Now
                    };
                    _context.ClubMemberships.Add(membership);
                    await _context.SaveChangesAsync();
                }
                #region feeamountchecks
                if (membership.FeeAmount == 50 && !member.IsStudent) //incorrect fee amount according to status
                {
                    membership.FeeAmount = 100;
                    _context.ClubMemberships.Update(membership);
                }
                if (membership.FeeAmount == 100 && member.IsStudent) //incorrect fee amount according to status
                {
                    membership.FeeAmount = 50;
                    _context.ClubMemberships.Update(membership);
                }
                #endregion             
                try
                {
                    _context.Members.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    string mesage = ex.Message;
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(member.MemberId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            
            return member;
        }

        public async Task<IEnumerable<Member>> Index(int? page, string searchString)
        {            
            var members = _context.Members.Include(m => m.RoleRole);
            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(m =>(m.Name +" "+m.Surname)!.Contains(searchString)).Include(m=>m.RoleRole);
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<Member> PagedList = new X.PagedList.PagedList<Member>(members, pageNumber, pageSize);
            return PagedList;
        }

        public bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        public bool MemberMailChecker(string email)
        {
            return _context.Members.Any(e => e.EmailAddress == email);
        }

        public SelectList GetRolesSelectedListItem(int RoleId)
        {
            return new SelectList(_context.Roles, "RoleId", "RoleName", RoleId);
        }
    }
}
