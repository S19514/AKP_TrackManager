using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;

namespace AKP_TrackManager.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public PaymentsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(int? page)
        {
            if (User.IsInRole("Admin"))
            {
                var aKP_TrackManager_devContext = _context.Payments.Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Payment> PagedList = new X.PagedList.PagedList<Payment>(await aKP_TrackManager_devContext.ToListAsync(), pageNumber, pageSize);
                return View(PagedList);
            }
            else
            {
                var currentMember = _context.Members.Where(m => m.EmailAddress == User.Identity.Name).FirstOrDefault();
                var aKP_TrackManager_devContext = _context.Payments.Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember).Where(c => c.MemberMemberId == currentMember.MemberId);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Payment> PagedList = new X.PagedList.PagedList<Payment>(await aKP_TrackManager_devContext.ToListAsync(), pageNumber, pageSize);
                return View("IndexAdmin",PagedList);
            }
        }
        [System.Web.Http.Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            if (User.IsInRole("Admin"))
            {
                var currentMember = _context.Members.Where(m => m.EmailAddress == User.Identity.Name).FirstOrDefault();
                var aKP_TrackManager_devContext = _context.Payments.Include(p => p.ClubMembershipMembership).Include(p => p.MemberMember).Where(c => c.MemberMemberId == currentMember.MemberId);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<Payment> PagedList = new X.PagedList.PagedList<Payment>(await aKP_TrackManager_devContext.ToListAsync(), pageNumber, pageSize);
                return View("IndexAdmin",PagedList);
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

            var payment = await _context.Payments
                .Include(p => p.ClubMembershipMembership)
                .Include(p => p.MemberMember)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        public IActionResult Create()
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
                ViewData["ClubMembershipMembershipId"] = new SelectList(_context.ClubMemberships, "MembershipId", "MembershipId");
                ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
                return View();
            }
            else
            {
                var currentMember = _context.Members.Where(m => m.EmailAddress == User.Identity.Name).FirstOrDefault();
                ViewData["ClubMembershipMembershipId"] = new SelectList(_context.ClubMemberships.Where(c => c.MemberMemberId == currentMember.MemberId), "MembershipId", "MembershipId");
                ViewData["MemberMemberId"] = new SelectList(_context.Members.Where(m => m.EmailAddress == User.Identity.Name), "MemberId", "EmailAddress");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,Amount,PaymentDate,MemberMemberId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var member = await _context.Members.FindAsync(payment.MemberMemberId);
                var membership = await _context.ClubMemberships.Where(m => m.MemberMemberId == member.MemberId).FirstOrDefaultAsync();
                payment.ClubMembershipMembershipId = membership.MembershipId;

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubMembershipMembershipId"] = new SelectList(_context.ClubMemberships, "MembershipId", "MembershipId", payment.ClubMembershipMembershipId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", payment.MemberMemberId);
            return View(payment);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["ClubMembershipMembershipId"] = new SelectList(_context.ClubMemberships, "MembershipId", "MembershipId", payment.ClubMembershipMembershipId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", payment.MemberMemberId);
            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,ClubMembershipMembershipId,Amount,PaymentDate,MemberMemberId")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["ClubMembershipMembershipId"] = new SelectList(_context.ClubMemberships, "MembershipId", "MembershipId", payment.ClubMembershipMembershipId);
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", payment.MemberMemberId);
            return View(payment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.ClubMembershipMembership)
                .Include(p => p.MemberMember)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.PaymentId == id);
        }
    }
}
