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

        public IActionResult Create()
        {
            ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Name,Surname,DateOfBirth,PhoneNumber,EmailAddress,IsAscendant,Password,IsStudent,RoleRoleId,IsBlocked")] Member member)
        {
            if (ModelState.IsValid)
            {
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleRoleId"] = new SelectList(_context.Roles, "RoleId", "RoleName", member.RoleRoleId);
            return View(member);
        }

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
                    _context.Update(member);
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.MemberId == id);
        }
    }
}
