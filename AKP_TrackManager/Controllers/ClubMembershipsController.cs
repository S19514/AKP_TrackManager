using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    [Authorize]
    public class ClubMembershipsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IMembershipRepository _membershipRepository;

        public ClubMembershipsController(AKP_TrackManager_devContext context, IMembershipRepository membershipRepository)
        {
            _context = context;
            _membershipRepository = membershipRepository;
        }

        public async Task<IActionResult> Index(int? page, string searchString)
        {
            ViewBag.SearchString = searchString;

            return View(await _membershipRepository.Index(page, User.Identity.Name, User.IsInRole("Admin"),searchString));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            
            return View("IndexAdmin",await _membershipRepository.IndexFilterAdmin(page,User.Identity.Name));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var clubMembership = await _membershipRepository.Details(id, User.Identity.Name, User.IsInRole("Admin"));
            if (clubMembership == null)
                return RedirectToAction(nameof(Index));
            else
                return View(clubMembership);
       
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["MemberMemberId"] = _membershipRepository.GetMembersSelectList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,JoinDate,FeeAmount,MemberMemberId")] ClubMembership clubMembership)
        {
            if (ModelState.IsValid)
            {
              var membership = await _membershipRepository.Create(new Models.DTO.MembershipCreateDto { Membership= clubMembership });
                if(membership == null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError(membership.ModelErrorKey, membership.ModelErrorString);
                ViewData["MemberMemberId"] = membership.ViewData;
            }
            ViewData["MemberMemberId"] = _membershipRepository.GetSelectedMemberSelectList(clubMembership.MemberMemberId);
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

            ViewData["MemberMemberId"] = _membershipRepository.GetSelectedMemberSelectList(clubMembership.MemberMemberId);
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
                var updatedMembership = await _membershipRepository.Edit(id,clubMembership, User.Identity.Name, User.IsInRole("Admin"));
                if (updatedMembership != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["MemberMemberId"] = _membershipRepository.GetSelectedMemberSelectList(clubMembership.MemberMemberId);
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
                return RedirectToAction(nameof(Index));
            }

            return View(clubMembership);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _membershipRepository.DeleteConfirmed(id, User.Identity.Name, User.IsInRole("Admin"));
            return RedirectToAction(nameof(Index));
        }
      
    }
}
