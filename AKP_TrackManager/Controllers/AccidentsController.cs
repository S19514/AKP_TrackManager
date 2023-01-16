using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    //[Authorize]
    public class AccidentsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IAccidentRepository _accidentRepository;        
        public AccidentsController(AKP_TrackManager_devContext context, IAccidentRepository accidentRepository)
        {
            _context = context;
            _accidentRepository = accidentRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? page, DateTime? searchString)
        {
            return View(await _accidentRepository.Index(page, User.Identity.Name, User.IsInRole("Admin"),searchString));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {            
               
                return View("IndexAdmin",await _accidentRepository.IndexFilterAdmin(page, User.Identity.Name));
            }
            else
            {
               return RedirectToAction(nameof(Index));
            }            
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            var accident = await _accidentRepository.Details(id, User.Identity.Name, User.IsInRole("Admin"));
            if (accident == null)
                return NotFound();
            return View(accident);
        }

        [Authorize]
        public IActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                ViewData["EmailAddress"] = new SelectList(_context.Members.Where(m=>m.EmailAddress == User.Identity.Name), "MemberId", "EmailAddress");
            }
            else
            {
                ViewData["EmailAddress"] = _accidentRepository.GetMembersSelectList();
                
            }
            ViewData["RegPlate"] = _accidentRepository.GetRegPlatesSelectList();
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AccidentId,AccidentDate,Severity,AnyoneInjured,MemberId,CarId")] AccidentCarMemberDto accident)
        {
            if (ModelState.IsValid)
            {
                if(await _accidentRepository.Create(accident) == null)
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmailAddress"] = _accidentRepository.GetMembersSelectList();
            ViewData["RegPlate"] = _accidentRepository.GetRegPlatesSelectList();
            return View(accident);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accidentToEdit = await _accidentRepository.Edit(id, User.Identity.Name, User.IsInRole("Admin"));
            if (accidentToEdit == null)
                return NotFound();
           
            ViewData["EmailAddress"] = _accidentRepository.GetMembersSelectList();
            ViewData["RegPlate"] = _accidentRepository.GetRegPlatesSelectList();
            return View(accidentToEdit);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("AccidentId,AccidentDate,Severity,AnyoneInjured,MemberId,CarId")] AccidentCarMemberDto accident)
        {
            if (id != accident.AccidentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               if(await _accidentRepository.Edit(id,accident, User.Identity.Name, User.IsInRole("Admin")) != null)
                return RedirectToAction(nameof(Index));
            }
            return View(accident);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var accidentCarMemberDto =  await _accidentRepository.Delete(id, User.Identity.Name, User.IsInRole("Admin"));
            if (accidentCarMemberDto == null)
                return NotFound();


            return View(accidentCarMemberDto);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _accidentRepository.DeleteConfirmed(id, User.Identity.Name, User.IsInRole("Admin"));
            return RedirectToAction(nameof(Index));
        }


    }
}
