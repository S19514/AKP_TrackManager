using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> Index(string contextUserName, bool isAdmin);
    }
}
