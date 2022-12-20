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
    public class LocationRepository : ILocationRepository
    {
        private AKP_TrackManager_devContext _context;
        public LocationRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<Location> Create(Location location)
        {
            try
            {
                _context.Add(location);
                await _context.SaveChangesAsync();
                return location;
            }
            catch(Exception ex) 
            {
                string message = ex.Message;
                return null;
            }
        }

        public async Task<bool> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            try
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex) 
            {
                string message = ex.Message;
                return false;
            }
        }

        public async Task<Location> Details(int? id)
        {
            var location = await _context.Locations
               .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return null;
            }
            return location;

        }

        public async Task<Location> Edit(int id, Location location)
        {
            try
            {
                _context.Update(location);
                await _context.SaveChangesAsync();
                return location;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(location.LocationId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<Location>> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<Location> PagedList = new X.PagedList.PagedList<Location>(await _context.Locations.Include(l=>l.training).ToListAsync(), pageNumber, pageSize);
            return PagedList;
        }

        public bool LocationExists(int id)
        {
            throw new NotImplementedException();
        }
    }
}
