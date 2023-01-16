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
    public class ConfigurationRepository : IConfigurationRepository
    {
        private AKP_TrackManager_devContext _context;
        public ConfigurationRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<TrackConfiguration> Create(TrackConfiguration trackConfiguration)
        {
            try
            {
                _context.Add(trackConfiguration);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                string message = ex.Message;
                return null;
            }

            return trackConfiguration;
        }

        public async Task<bool> DeleteConfirmed(int id)
        {
            var trackConfiguration = await _context.TrackConfigurations.FindAsync(id);
            try
            {
                _context.TrackConfigurations.Remove(trackConfiguration);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return false;
            }            
           
        }

        public async Task<TrackConfiguration> Details(int? id)
        {
            var trackConfiguration = await _context.TrackConfigurations
              .FirstOrDefaultAsync(m => m.TrackId == id);
            if (trackConfiguration == null)
            {
                return null;
            }
            return trackConfiguration;
        }

        public async Task<TrackConfiguration> Edit(int id, TrackConfiguration trackConfiguration)
        {
            try
            {
                _context.TrackConfigurations.Update(trackConfiguration);
                await _context.SaveChangesAsync();
                return trackConfiguration;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackConfigurationExists(trackConfiguration.TrackId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }            
        }

        public async Task<IEnumerable<TrackConfiguration>> Index(int? page,string searchName, int? searchNumber)
        {
            var configurations = await _context.TrackConfigurations.Include(t => t.training).ToListAsync();
            if(!String.IsNullOrEmpty(searchName))
            {
                configurations = configurations.Where(c=> c.PresetName!.Contains(searchName)).ToList();
            }
            if (searchNumber != null && searchNumber > 0)
            {
                configurations = configurations.Where(c => c.PresetNumber!.Equals(searchNumber)).ToList();
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<TrackConfiguration> PagedList = new X.PagedList.PagedList<TrackConfiguration>(configurations, pageNumber, pageSize);
            return PagedList;
        }

        public bool TrackConfigurationExists(int id)
        {
            return _context.TrackConfigurations.Any(e => e.TrackId == id);
        }
    }
}
