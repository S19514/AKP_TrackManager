using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface IConfigurationRepository
    {
        Task<IEnumerable<TrackConfiguration>> Index(int? page, string searchName, int? searchNumber);        
        Task<TrackConfiguration> Details(int? id);
        Task<TrackConfiguration> Create(TrackConfiguration trackConfiguration);
        Task<TrackConfiguration> Edit(int id, TrackConfiguration trackConfiguration);        
        Task<bool> DeleteConfirmed(int id);
        bool TrackConfigurationExists(int id);
    }
}
