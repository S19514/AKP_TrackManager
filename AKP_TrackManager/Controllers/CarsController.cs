using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using AKP_TrackManager.Interfaces;

namespace AKP_TrackManager.Controllers
{
    [Authorize]
    public class CarsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private ICarRepository _carRepository;
        public CarsController(AKP_TrackManager_devContext context, ICarRepository carRepository)
        {
            _context = context;
            _carRepository = carRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {
            return View(await _carRepository.Index(page, User.Identity.Name, User.IsInRole("Admin")));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            if (HttpContext.User.IsInRole("Admin"))
            {
        
                return View("IndexAdmin", await _carRepository.IndexFilterAdmin(page, User.Identity.Name));                
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
           var carMember = await _carRepository.Details(id,User.Identity.Name,User.IsInRole("Admin"));
            if(carMember == null)
            {
                return NotFound();
            }

            return View(carMember);
        }

        public IActionResult Create()
        {            
            ViewData["MemberMemberId"] = _carRepository.GetMembersSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarId,Make,Model,EngingeCapacity,EnginePower,RegPlate,MemberId")] CarMemberDto pCarMember)
        {
            if (ModelState.IsValid)
            {
             if(await _carRepository.Create(pCarMember) == null)
                return RedirectToAction(nameof(Index));

            }
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress",pCarMember.MemberId);
            return View(pCarMember);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["MemberMemberId"] = _carRepository.GetMembersSelectList();

            var carMemberDto = await _carRepository.Edit(id, User.Identity.Name, User.IsInRole("Admin"));
            if(carMemberDto == null)
            {
                return NotFound();
            }
            return View(carMemberDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarId,Make,Model,EngingeCapacity,EnginePower,RegPlate,MemberId")] CarMemberDto pCarMemberDto)
        {
            if (id != pCarMemberDto.CarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedCarMember = await _carRepository.Edit(id,pCarMemberDto, User.Identity.Name, User.IsInRole("Admin"));
                if (updatedCarMember != null)
                {
                    return RedirectToAction(nameof(Index));
                }               
                else
                {
                    return RedirectToAction(nameof(Index));
                    //potential place for displaying error 
                }
            }
            ViewData["MemberMemberId"] = _carRepository.GetSelectedMemberSelectList(pCarMemberDto.MemberId);      
            return View(pCarMemberDto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.CarId == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _carRepository.DeleteConfirmed(id,User.Identity.Name,User.IsInRole("Admin"));

            return RedirectToAction(nameof(Index));
        }

    }
}
