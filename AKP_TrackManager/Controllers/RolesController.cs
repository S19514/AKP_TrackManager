using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IRoleRepository _roleRepository;

        public RolesController(AKP_TrackManager_devContext context, IRoleRepository roleRepository)
        {
            _context = context;
            _roleRepository = roleRepository;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _roleRepository.Index(User.Identity.Name,User.IsInRole("Admin")));
        }
        #region not used
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var role = await _context.Roles
        //        .FirstOrDefaultAsync(m => m.RoleId == id);
        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(role);
        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("RoleId,RoleName")] Role role)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(role);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(role);
        //}

        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var role = await _context.Roles.FindAsync(id);
        //    if (role == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(role);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("RoleId,RoleName")] Role role)
        //{
        //    if (id != role.RoleId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(role);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!RoleExists(role.RoleId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(role);
        //}

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var role = await _context.Roles
        //        .FirstOrDefaultAsync(m => m.RoleId == id);
        //    if (role == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(role);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var role = await _context.Roles.FindAsync(id);
        //    _context.Roles.Remove(role);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool RoleExists(int id)
        //{
        //    return _context.Roles.Any(e => e.RoleId == id);
        //}
        #endregion
    }
}
