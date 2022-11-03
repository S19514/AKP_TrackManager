using AKP_TrackManager.Interfaces;
using AKP_TrackManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKP_TrackManager.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private AKP_TrackManager_devContext _context;
        public MemberRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }
        public IActionResult Create()
        {
            throw new System.NotImplementedException();            
        }

        public Task<IActionResult> Create(Member member)
        {
            throw new System.NotImplementedException();
        }

        public Task<IActionResult> Delete(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IActionResult> DeleteConfirmed(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IActionResult> Details(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IActionResult> Edit(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IActionResult> Edit(int id, Member member)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Member>> Index()
        {
            var members = await _context.Members.ToListAsync();
            return members;
        }

        public bool MemberExists(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
