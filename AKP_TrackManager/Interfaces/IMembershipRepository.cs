using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
namespace AKP_TrackManager.Interfaces
{
    public interface IMembershipRepository
    {
        Task<IEnumerable<ClubMembership>> Index(int? page, string contextUserName, bool isAdmin);
        Task<IEnumerable<ClubMembership>> IndexFilterAdmin(int? page, string contextUserName);
        Task<ClubMembership> Details(int? id, string contextUserName, bool isAdmin);
        SelectList GetMembersSelectList();
        SelectList GetSelectedMemberSelectList(int memberId);
        Task<MembershipCreateDto> Create(MembershipCreateDto clubMembership);
        Task<ClubMembership> Edit(int id, ClubMembership clubMembership, string contextUserName, bool isAdmin);
        Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin);
        bool ClubMembershipExists(int id);
    }
}
