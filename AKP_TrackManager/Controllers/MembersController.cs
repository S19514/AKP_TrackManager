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
            return View(await _memberRepository.Index(page));            
        }

        public async Task<IActionResult> Details(int? id)
        {
            var member =  await _memberRepository.Details(id);
            if (member == null)
                return NotFound();
            else
                return View(member);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["RoleRoleId"] = _memberRepository.Create();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,Name,Surname,DateOfBirth,PhoneNumber,EmailAddress,IsAscendant,Password,IsStudent,RoleRoleId,IsBlocked")] Member member)
        {
            if (ModelState.IsValid)
            {
                var newMember = await _memberRepository.Create(new Models.DTO.MemberCreateDto { Member = member });
                if ( newMember.Member != null)
                {
                    ModelState.AddModelError(newMember.ModelErrorKey, newMember.ModelErrorString);
                    ViewData["RoleRoleId"] = newMember.ViewData;
                    return View(newMember.Member);
                }                
            }            
            ViewData["RoleRoleId"] = _memberRepository.GetRolesSelectedListItem(member.RoleRoleId);
            return View(member);
        }
       
        [Authorize]
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
            if (member.EmailAddress != User.Identity.Name && !User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["RoleRoleId"] = _memberRepository.GetRolesSelectedListItem(member.RoleRoleId);
            return View(member);
        }
       
        [Authorize]
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
                var memberUpdate = await _memberRepository.Edit(id, member);
                if (memberUpdate == null)
                    return NotFound();
                else
                    return RedirectToAction(nameof(Index));                
            }
            ViewData["RoleRoleId"] = _memberRepository.GetRolesSelectedListItem(member.RoleRoleId);
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
            _ = await _memberRepository.DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
