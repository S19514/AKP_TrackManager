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
    public class PaymentsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;
        private IPaymentRepository _paymentRepository;
        public PaymentsController(AKP_TrackManager_devContext context, IPaymentRepository paymentRepository)
        {
            _context = context;
            _paymentRepository = paymentRepository;
        }
        
        public async Task<IActionResult> Index(int? page, DateTime? searchDate, string searchString)
        {
            return View(await _paymentRepository.Index(page, User.Identity.Name, User.IsInRole("Admin"),searchDate,searchString));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexFilterAdmin(int? page)
        {
            var payments = await _paymentRepository.IndexFilterAdmin(page, User.Identity.Name);
            if (payments == null)
                return RedirectToAction(nameof(Index));

            return View("IndexAdmin",payments);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var paymentDetails = await _paymentRepository.Details(id, User.Identity.Name, User.IsInRole("Admin"));
            if (paymentDetails == null)
                return RedirectToAction(nameof(Index));

            return View(paymentDetails);           
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ClubMembershipMembershipId"] = _paymentRepository.GetMembershipsSelectList();
            ViewData["MemberMemberId"] = _paymentRepository.GetMembersSelectList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,Amount,PaymentDate,MemberMemberId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                var newPayment = await _paymentRepository.Create(payment);
                if(newPayment != null)
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubMembershipMembershipId"] = _paymentRepository.GetSelectedMembershipSelectList(payment.ClubMembershipMembershipId);
            ViewData["MemberMemberId"] = _paymentRepository.GetSelectedMemberSelectList(payment.MemberMemberId);
            return View(payment);
        }        

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _ = await _paymentRepository.DeleteConfirmed(id, User.Identity.Name, User.IsInRole("Admin"));
            return RedirectToAction(nameof(Index));
        }

        #region not used
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var payment = await _context.Payments.FindAsync(id);
            //if (payment == null)
            //{
            //    return NotFound();
            //}
            //ViewData["ClubMembershipMembershipId"] = _paymentRepository.GetSelectedMembershipSelectList(payment.ClubMembershipMembershipId);
            //ViewData["MemberMemberId"] = _paymentRepository.GetSelectedMemberSelectList(payment.MemberMemberId);
            //return View(payment);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,ClubMembershipMembershipId,Amount,PaymentDate,MemberMemberId")] Payment payment)
        {
            //if (id != payment.PaymentId)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(payment);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!PaymentExists(payment.PaymentId))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["ClubMembershipMembershipId"] = _paymentRepository.GetSelectedMembershipSelectList(payment.ClubMembershipMembershipId);
            //ViewData["MemberMemberId"] = _paymentRepository.GetSelectedMemberSelectList(payment.MemberMemberId);
            //return View(payment);
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
