using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface ICarRepository
    {
        Task<IEnumerable<CarMemberDto>> Index(int? page, string contextUserName, bool isAdmin);
        Task<IEnumerable<CarMemberDto>> IndexFilterAdmin(int? page, string contextUserName);
        Task<CarMemberDto> Details(int? id, string contextUserName, bool isAdmin);
        SelectList GetMembersSelectList();
        SelectList GetSelectedMemberSelectList(int memberId);
        Task<CarMemberDto> Create(CarMemberDto pCarMember);
        Task<CarMemberDto> Edit(int id, CarMemberDto pCarMemberDto, string contextUserName, bool isAdmin);
        Task<CarMemberDto> Edit(int? id, string contextUserName, bool isAdmin);
        Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin);
        bool CarExists(int id);

    }
}
