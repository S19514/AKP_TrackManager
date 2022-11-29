using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using AKP_TrackManager.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Server.IISIntegration;
using AKP_TrackManager.Security;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AKP_TrackManager.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IMemberRepository _memberRepository;

        public MembersController(AKP_TrackManager_devContext context, IMemberRepository memberRepository)
        {
            _context = context;
            _memberRepository = memberRepository;            
        }

        public async Task<IActionResult> Index(int? page)
        {
            var members = _context.Members.Include(m => m.RoleRole);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<Member> PagedList = new X.PagedList.PagedList<Member>(members, pageNumber, pageSize);
            return View(PagedList);            
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.RoleRole)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Name,Surname,DateOfBirth,PhoneNumber,EmailAddress,IsAscendant,Password,IsStudent,RoleRoleId,IsBlocked")] Member member)
        {
            if (ModelState.IsValid)
            {
                if(MemberMailChecker(member.EmailAddress))
                {
                    ModelState.AddModelError("Email exists", "Email is taken");                    
                    ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", member.RoleRoleId);
                    return View(member);
                }
                _context.Members.Add(member);
                await _context.SaveChangesAsync();

                ClubMembership membership = new ClubMembership()
                {
                    FeeAmount = member.IsStudent ? 50 : 100,
                    JoinDate = DateTime.Now,
                    MemberMemberId= member.MemberId,
                    
                };
                _context.ClubMemberships.Add(membership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", member.RoleRoleId);
            return View(member);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", member.RoleRoleId);
            return View(member);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,Name,Surname,DateOfBirth,PhoneNumber,EmailAddress,IsAscendant,Password,IsStudent,RoleRoleId,IsBlocked")] Member member)
        {
            if (id != member.MemberId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var membership = await _context.ClubMemberships.Where(m=>m.MemberMemberId== member.MemberId).FirstOrDefaultAsync();
                    if(membership == null) 
                    {
                        ClubMembership newMembership = new ClubMembership()
                        {
                            FeeAmount = member.IsStudent ? 50 : 100,
                            JoinDate = DateTime.Now
                        };
                        _context.ClubMemberships.Add(membership);
                        await _context.SaveChangesAsync();
                    }
                    if(membership.FeeAmount == 50 && !member.IsStudent) //incorrect fee amount according to status
                    {
                        membership.FeeAmount = 100;
                        _context.ClubMemberships.Update(membership);
                    }
                    if(membership.FeeAmount == 100 && member.IsStudent) //incorrect fee amount according to status
                    {
                        membership.FeeAmount = 50;
                        _context.ClubMemberships.Update(membership);
                    }
                    _context.Members.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.MemberId))
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
            ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", member.RoleRoleId);
            return View(member);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.RoleRole)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            var membership = await _context.ClubMemberships.Where(m=>m.MemberMemberId == id).FirstOrDefaultAsync();
            if(membership != null)
            {            
                var payments = await _context.Payments.Where(p=>p.ClubMembershipMembershipId == membership.MembershipId).ToListAsync();

                foreach(var payment in payments)
                {
                    _context.Payments.Remove(payment);
                }           
                _context.ClubMemberships.Remove(membership);
            }
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }

        private bool MemberMailChecker(string email)
        {
            return _context.Members.Any(e => e.EmailAddress == email);
        }
    }
}
