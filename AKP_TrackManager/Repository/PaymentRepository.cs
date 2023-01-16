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
    public class PaymentRepository : IPaymentRepository
    {
        private AKP_TrackManager_devContext _context;
        public PaymentRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<Payment> Create(Payment payment)
        {
            var member = await _context.Members.FindAsync(payment.MemberMemberId);
            if (member == null)
            {
                return null;
            }
            var membership = await _context.ClubMemberships.Where(m => m.MemberMemberId == member.MemberId).FirstOrDefaultAsync();
            if (membership == null)
            {
                return null;
            }
            payment.ClubMembershipMembershipId = membership.MembershipId;

            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return null;
            }
            return payment;
        }

        public async Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin)
        {
            var payment = await _context.Payments.FindAsync(id);
            try
            {
                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return false;
            }
            return true;
        }

        public async Task<Payment> Details(int? id, string contextUserName, bool isAdmin)
        {
            var payment = await _context.Payments.Include(p => p.ClubMembershipMembership)
                                                .Include(p => p.MemberMember)
                                                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return null;
            }

            if (contextUserName == payment.MemberMember.EmailAddress || isAdmin)
            {
                return payment;
            }
            else
            {
                return null;
            }
        }

        public SelectList GetMembershipsSelectList()
        {
            return new SelectList(_context.ClubMemberships, "MembershipId", "MembershipId");
        }

        public SelectList GetMembersSelectList()
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress");
        }

        public SelectList GetSelectedMemberSelectList(int memberId)
        {
            return new SelectList(_context.Members, "MemberId", "EmailAddress", memberId);
        }

        public SelectList GetSelectedMembershipSelectList(int membershipId)
        {
            return new SelectList(_context.ClubMemberships, "MembershipId", "MembershipId", membershipId);
        }

        public async Task<IEnumerable<Payment>> Index(int? page, string contextUserName, bool isAdmin, DateTime? searchDate, string searchString)
        {
            if (isAdmin)
            {
                var aKP_TrackManager_devContext = _context.Payments.Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember);
                if(searchDate != null)
                {
                    aKP_TrackManager_devContext = aKP_TrackManager_devContext.Where(p=>p.PaymentDate!.Equals(searchDate)).Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember);
                }
                if(!String.IsNullOrEmpty(searchString))
                {
                    aKP_TrackManager_devContext = aKP_TrackManager_devContext.Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember).Where(p => (p.MemberMember.Name + " " + p.MemberMember.Surname)!.Contains(searchString)).Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember);
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Payment> PagedList = new X.PagedList.PagedList<Payment>(await aKP_TrackManager_devContext.ToListAsync(), pageNumber, pageSize);
                return PagedList;
            }
            else
            {
                var currentMember = _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefault();
                var payments = await _context.Payments.Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember).Where(c => c.MemberMemberId == currentMember.MemberId).ToListAsync();
                if (searchDate != null)
                {
                    payments = payments.Where(p => p.PaymentDate!.Equals(searchDate)).ToList();
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    payments = payments.Where(p => (p.MemberMember.Name + " " + p.MemberMember.Surname)!.Contains(searchString)).ToList();
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Payment> PagedList = new X.PagedList.PagedList<Payment>(payments, pageNumber, pageSize);
                return PagedList;
            }
        }

        public async Task<IEnumerable<Payment>> IndexFilterAdmin(int? page, string contextUserName)
        {
            var currentMember = await _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefaultAsync();
            if (currentMember == null)
                return null;
            var aKP_TrackManager_devContext = _context.Payments.Include(p => p.ClubMembershipMembership)
                                                                .Include(p => p.MemberMember)
                                                                .Where(c => c.MemberMemberId == currentMember.MemberId);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<Payment> PagedList = new X.PagedList.PagedList<Payment>(await aKP_TrackManager_devContext.ToListAsync(), pageNumber, pageSize);
            return PagedList;
        }

        public bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
