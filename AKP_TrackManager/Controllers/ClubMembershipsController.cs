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
    public class ClubMembershipsController : Controller
    {
        private readonly AKP_TrackManager_devContext _context;

        public ClubMembershipsController(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        // GET: ClubMemberships
        public async Task<IActionResult> Index()
        {
            var aKP_TrackManager_devContext = _context.ClubMemberships.Include(c => c.MemberMember);
            return View(await aKP_TrackManager_devContext.ToListAsync());
        }

        // GET: ClubMemberships/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: ClubMemberships/Create
        public IActionResult Create()
        {
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress");
            return View();
        }

        // POST: ClubMemberships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MembershipId,JoinDate,FeeAmount,MemberMemberId")] ClubMembership clubMembership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(clubMembership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MemberMemberId"] = new SelectList(_context.Members, "MemberId", "EmailAddress", clubMembership.MemberMemberId);
            return View(clubMembership);
        }

        // GET: ClubMemberships/Edit/5
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

        // POST: ClubMemberships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MembershipId,JoinDate,FeeAmount,MemberMemberId")] ClubMembership clubMembership)
        {
            if (id != clubMembership.MembershipId)
            {
                return NotFound();
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

        // GET: ClubMemberships/Delete/5
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

        // POST: ClubMemberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clubMembership = await _context.ClubMemberships.FindAsync(id);
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
