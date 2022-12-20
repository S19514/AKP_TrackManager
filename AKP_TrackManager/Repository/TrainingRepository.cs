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
    public class TrainingRepository : ITrainingRepository
    {
        private AKP_TrackManager_devContext _context;
        public TrainingRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<training>> Index(int? page)
        {
            var aKP_TrackManager_devContext = _context.training.OrderByDescending(t => t.Date)
                                                                .Include(t => t.LocationLocation)
                                                                .Include(t => t.TrackConfigurationTrack);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<training> PagedList = new X.PagedList.PagedList<training>(aKP_TrackManager_devContext, pageNumber, pageSize);
            return  PagedList;
        }

        public async Task<training> Details(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var training = await _context.training
                .Include(t => t.LocationLocation)
                .Include(t => t.TrackConfigurationTrack)
                .Include(t => t.TrainingAttandances)
                .FirstOrDefaultAsync(m => m.TrainingId == id);

            foreach (var ta in training.TrainingAttandances)
            {
                ta.MemberMember = await _context.Members.FirstAsync(m => m.MemberId == ta.MemberMemberId);
            }
            if (training == null)
            {
                return null;
            }
            return training;
        }

        public SelectList GetLocationFriendlyNames()
        {
            return new SelectList(_context.Locations, "LocationId", "FriendlyName");
        }

        public SelectList GetTrackConfigurationPresetNames()
        {
            return new SelectList(_context.TrackConfigurations, "TrackId", "PresetName");
        }

        public async Task<bool> SingUpForTraining(int? id, string contextUserName)
        {
            var training = _context.training.Where(t => t.TrainingId == id).FirstOrDefault();
            var member = _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefault();
            var trainingAttandanceHistory = _context.TrainingAttandances
                                                    .Where(ta => ta.MemberMemberId == member.MemberId && ta.TrainingTrainingId == training.TrainingId)
                                                    .FirstOrDefault();
            if (trainingAttandanceHistory == null)
            {
                var trainingAttandance = new TrainingAttandance
                {
                    MemberMemberId = member.MemberId,
                    TrainingTrainingId = training.TrainingId,
                };

                _context.Add(trainingAttandance);
                await _context.SaveChangesAsync();                
                return true;
            }
            return false;
        }

        public async Task<TrainingCreateDto> Create(TrainingCreateDto training)
        {
            _context.training.Add(training.Training);
            try
            {
                await _context.SaveChangesAsync();
            }catch(Exception ex)
            {
                training.ModelErrorKey = "error occured";
                training.ModelErrorString = ex.Message;
            }
            training.Training = null;
            return training;
        }

        public async Task<training> Edit(int id, training training)
        {
            try
            {
                _context.Update(training);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!trainingExists(training.TrainingId))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return training;
        }

        public async Task<bool> DeleteConfirmed(int id)
        {
            var training = await _context.training.FindAsync(id);
            _context.training.Remove(training);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                string exMessage = ex.Message;
                //possibility of logging errors
                return false;
            }
            return true;
        }  

        public bool trainingExists(int id)
        {
            return _context.training.Any(e => e.TrainingId == id);
        }

        public SelectList GetSelectedLocationFriendlyNames(int? locationId)
        {
            return new SelectList(_context.Locations, "LocationId", "FriendlyName", locationId);
        }

        public SelectList GetSelectedTrackConfigurationPresetNames(int? trackConfigurationId)
        {
            return new SelectList(_context.TrackConfigurations, "TrackId", "PresetName", trackConfigurationId);
        }
    }
}
