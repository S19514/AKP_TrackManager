using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKP_TrackManager.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;

namespace AKP_TrackManager.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private AKP_TrackManager_devContext _context;
        public RoleRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> Index(string contextUserName, bool isAdmin)
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
