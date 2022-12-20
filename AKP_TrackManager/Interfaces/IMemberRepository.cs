using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKP_TrackManager.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> Index(int? page);
        Task<Member> Details(int? id);
        SelectList Create();
        Task<MemberCreateDto> Create(MemberCreateDto member);
        Task<Member> Edit(int id, Member member);
        Task<bool> DeleteConfirmed(int id);
        bool MemberExists(int id);
        bool MemberMailChecker(string email);
        SelectList GetRolesSelectedListItem(int RoleId);
    }
}
