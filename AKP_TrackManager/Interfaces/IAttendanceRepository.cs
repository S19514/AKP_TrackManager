using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<TrainingAttandance>> Index(int? page, string contextUserName, bool isAdmin);
        Task<IEnumerable<TrainingAttandance>> IndexFilterAdmin(int? page, string contextUserName);
        Task<TrainingAttandance> Details(int? id, string contextUserName, bool isAdmin);
        Task<int> DeleteConfirmed(int id, string contextUserName, bool isAdmin);
        bool TrainingAttandanceExists(int id);
    }
}
