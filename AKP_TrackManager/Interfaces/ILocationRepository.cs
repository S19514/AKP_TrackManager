using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> Index(int? page, string searchString);        
        Task<Location> Details(int? id);
        Task<Location> Create(Location location);
        Task<Location> Edit(int id, Location location);        
        Task<bool> DeleteConfirmed(int id);
        bool LocationExists(int id);
    }
}
