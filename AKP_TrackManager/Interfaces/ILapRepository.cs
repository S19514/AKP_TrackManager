using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface ILapRepository
    {
        Task<IEnumerable<MemberCarOnLapDto>> Index(int? page, string contextUserName, bool isAdmin);
        Task<IEnumerable<MemberCarOnLapDto>> IndexFilterAdmin(int? page, string contextUserName);
        Task<IEnumerable<MemberCarOnLapDto>> IndexByTrainingId(int? id,int? page, string contextUserName);
        Task<MemberCarOnLapDto> Details(int? id, string contextUserName, bool isAdmin);
        SelectList GetMembersSelectList();
        SelectList GetSelectedMemberSelectList(int memberId);
        SelectList GetRegPlatesSelectList();
        SelectList GetSelectedRegPlatesSelectList(int carId);
        Task<MemberCarOnLapDto> Create(MemberCarOnLapDto pMemberLapOnCar);
        Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin);    
    }
}
