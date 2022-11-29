using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace AKP_TrackManager.Controllers
{
    [Authorize]
    public class ClubMembershipsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public ClubMembershipsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? page)
        {
            if (User.IsInRole("Admin"))
            {
                var aKP_TrackManager_devContext = _context.ClubMemberships.Include(c => c.MemberMember);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<ClubMembership> PagedList = new X.PagedList.PagedList<ClubMembership>(await aKP_TrackManager_devContext.ToListAsync(), pageNumber, pageSize);
                return View(PagedList);
            }
            else
            {
                var member = await _context.Members.Where(m => m.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
                var aKP_TrackManager_devContext = await _context.ClubMemberships.Include(c => c.MemberMember).Where(m => m.MemberMemberId == member.MemberId).ToListAsync();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                X.PagedList.PagedList<ClubMembership> PagedList = new X.PagedList.PagedList<ClubMembership>(aKP_TrackManager_devContext, pageNumber, pageSize);
                return View(PagedList);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            var member = await _context.Members.Where(m => m.EmailAddress == User.Identity.Name).FirstOrDefaultAsync();
            var aKP_TrackManager_devContext = await _context.ClubMemberships.Include(c => c.MemberMember).Where(m => m.MemberMemberId == member.MemberId).ToListAsync();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<ClubMembership> PagedList = new X.PagedList.PagedList<ClubMembership>(aKP_TrackManager_devContext, pageNumber, pageSize);
            return View("IndexAdmin",PagedList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMembership = await _context.ClubMemberships
                .Include(c => c.MemberMember)
                .Include(p=>p.Payments)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (clubMembership == null)
            {
                return NotFound();
            }

            if (HttpContext.User.Identity.Name == clubMembership.MemberMember.EmailAddress)
            {
                return View(clubMembership);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,JoinDate,FeeAmount,MemberMemberId")] ClubMembership clubMembership)
        {
            if (ModelState.IsValid)
            {
                var member = await _context.Members.FindAsync(clubMembership.MemberMemberId);
                var memberships = await _context.ClubMemberships.Where(m => m.MemberMemberId == member.MemberId).FirstOrDefaultAsync();
                if(memberships != null)
                {
                    ModelState.AddModelError("Member already has membership", "Cannot assign more than 1 membership to member");
                    ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", clubMembership.MemberMemberId);
                    return View(clubMembership);
                }
                _context.Add(clubMembership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", clubMembership.MemberMemberId);
            return View(clubMembership);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var clubMembership = await _context.ClubMemberships.FindAsync(id);
            if (clubMembership == null)
            {
                return NotFound();
            }            

            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", clubMembership.MemberMemberId);
            return View(clubMembership);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipId,JoinDate,FeeAmount,MemberMemberId")] ClubMembership clubMembership)
        {
            if (id != clubMembership.MembershipId)
            {
                return NotFound();
            }
            var membershipByMember = await _context.ClubMemberships.Where(m => m.MemberMemberId == clubMembership.MemberMemberId).FirstOrDefaultAsync();
            if(membershipByMember!= null && membershipByMember.MembershipId != clubMembership.MembershipId)
            {
                ModelState.AddModelError("Member already has membership", "Cannot assign more than 1 membership to member");
                ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", clubMembership.MemberMemberId);
                return View(clubMembership);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clubMembership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubMembershipExists(clubMembership.MembershipId))
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
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", clubMembership.MemberMemberId);
            return View(clubMembership);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clubMembership = await _context.ClubMemberships
                .Include(c => c.MemberMember)
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (clubMembership == null)
            {
                return NotFound();
            }

            return View(clubMembership);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clubMembership = await _context.ClubMemberships.FindAsync(id);
            var payments = await _context.Payments.Where(c=> c.ClubMembershipMembershipId== id).ToListAsync();
            foreach (var payment in payments)
            {
                _context.Payments.Remove(payment);
            }
            _context.ClubMemberships.Remove(clubMembership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubMembershipExists(int id)
        {
            return _context.ClubMemberships.Any(e => e.MembershipId == id);
        }
    }
}
