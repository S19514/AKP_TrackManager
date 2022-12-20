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
    public class AttendanceRepository : IAttendanceRepository
    {
        private AKP_TrackManager_devContext _context;
        public AttendanceRepository(AKP_TrackManager_devContext context)
        {
            _context = context;
        }

        public async Task<int> DeleteConfirmed(int id, string contextUserName, bool isAdmin)
        {
            var trainingAttandance = await _context.TrainingAttandances.FindAsync(id);
            var trainingId = trainingAttandance.TrainingTrainingId;
            var member = await _context.Members.FindAsync(trainingAttandance.MemberMemberId);
            if (member == null)
                return 0;
            
            if (member.EmailAddress != contextUserName)
                return 0;

            _context.TrainingAttandances.Remove(trainingAttandance);
            await _context.SaveChangesAsync();
            return trainingId;
        }

        public async Task<TrainingAttandance> Details(int? id, string contextUserName, bool isAdmin)
        {
            var trainingAttandance = await _context.TrainingAttandances
                .Include(t => t.MemberMember)
                .Include(t => t.TrainingTraining)
                .Include(t => t.TrainingTraining.LocationLocation)
                .FirstOrDefaultAsync(m => m.TrainingAttandanceId == id);
            if (trainingAttandance == null)
            {
                return null;
            }
            return trainingAttandance;
        }

        public async Task<IEnumerable<TrainingAttandance>> Index(int? page, string contextUserName, bool isAdmin)
        {

            var trainingAttendances = await _context.TrainingAttandances
                                                        .Include(t => t.MemberMember)
                                                        .Include(t => t.TrainingTraining)
                                                        .Include(t => t.TrainingTraining.LocationLocation).OrderByDescending(t => t.TrainingAttandanceId).ToListAsync();            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<TrainingAttandance> PagedList = new X.PagedList.PagedList<TrainingAttandance>( trainingAttendances, pageNumber, pageSize);
            return PagedList;

        }

        public async Task<IEnumerable<TrainingAttandance>> IndexFilterAdmin(int? page, string contextUserName)
        {
            var member = _context.Members.Where(m => m.EmailAddress == contextUserName).FirstOrDefault();
            var trainingAttendances = await _context.TrainingAttandances
                                                        .Include(t => t.MemberMember)
                                                        .Include(t => t.TrainingTraining)
                                                        .Include(t => t.TrainingTraining.LocationLocation)
                                                        .Where(t => t.MemberMemberId == member.MemberId).OrderByDescending(t => t.TrainingAttandanceId).ToListAsync();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            X.PagedList.PagedList<TrainingAttandance> PagedList = new X.PagedList.PagedList<TrainingAttandance>(trainingAttendances, pageNumber, pageSize);
            return PagedList;
        }

        public bool TrainingAttandanceExists(int id)
        {
            return _context.TrainingAttandances.Any(e => e.TrainingAttandanceId == id);
        }
    }
}
