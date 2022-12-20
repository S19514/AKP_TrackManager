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
    public class MembershipRepository : IMembershipRepository
    {
        private AKP_TrackManager_devContext _context;
        public MembershipRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public bool ClubMembershipExists(int id)
        {
            return _context.ClubMemberships.Any(e => e.MembershipId == id);
        }

        public async Task<MembershipCreateDto> Create(MembershipCreateDto clubMembership)
        {
            var member = await _context.Members.FindAsync(clubMembership.Membership.MemberMemberId);
            var memberships = await _context.ClubMemberships.Where(m => m.MemberMemberId == member.MemberId).FirstOrDefaultAsync();
            if (memberships != null)
            {
                clubMembership.ModelErrorKey = "Member already has membership";
                clubMembership.ModelErrorString = "Cannot assign more than 1 membership to member";
               clubMembership.ViewData = GetSelectedMemberSelectList(clubMembership.Membership.MemberMemberId);
                return clubMembership;
            }
            _context.ClubMemberships.Add(clubMembership.Membership);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin)
        {
            var clubMembership = await _context.ClubMemberships.FindAsync(id);
            var payments = await _context.Payments.Where(c => c.ClubMembershipMembershipId == id).ToListAsync();
            foreach (var payment in payments)
            {
                _context.Payments.Remove(payment);
            }
            _context.ClubMemberships.Remove(clubMembership);
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

        public async Task<ClubMembership> Details(int? id, string contextUserName, bool isAdmin)
        {
            var clubMembership = await _context.ClubMemberships
           .Include(c => c.MemberMember)
           .Include(p => p.Payments)
           .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (clubMembership == null)
            {
                return null;
            }

            if (contextUserName == clubMembership.MemberMember.EmailAddress ||isAdmin)
            {
                return clubMembership;
            }
            else
            {
                return null;
            }
        }

        public async Task<ClubMembership> Edit(int id, ClubMembership clubMembership, string contextUserName, bool isAdmin)
        {
            try
            {
                _context.ClubMemberships.Update(clubMembership);
                await _context.SaveChangesAsync();
                return clubMembership;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubMembershipExists(clubMembership.MembershipId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public SelectList GetMembersSelectList()
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress");
        }

        public SelectList GetSelectedMemberSelectList(int memberId)
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress", memberId);
        }

        public async Task<IEnumerable<ClubMembership>> Index(int? page, string contextUserName, bool isAdmin)
        {
            if (isAdmin)
            {
                var memberships = _context.ClubMemberships.Include(c => c.MemberMember);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<ClubMembership> PagedList = new X.PagedList.PagedList<ClubMembership>(await memberships.ToListAsync(), pageNumber, pageSize);
                return PagedList;
            }
            else
            {
                var member = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
                var memberships = await _context.ClubMemberships.Include(c => c.MemberMember).Where(m => m.MemberMemberId == member.MemberId).ToListAsync();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<ClubMembership> PagedList = new X.PagedList.PagedList<ClubMembership>(memberships, pageNumber, pageSize);
                return PagedList;
            }
        }

        public async Task<IEnumerable<ClubMembership>> IndexFilterAdmin(int? page, string contextUserName)
        {
            var member = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
            var memberships = await _context.ClubMemberships.Include(c => c.MemberMember).Where(m => m.MemberMemberId == member.MemberId).ToListAsync();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<ClubMembership> PagedList = new X.PagedList.PagedList<ClubMembership>(memberships, pageNumber, pageSize);
            return PagedList;
        }
    }
}
