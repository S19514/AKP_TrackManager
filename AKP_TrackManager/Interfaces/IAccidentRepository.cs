using AKP_TrackManager.Models.DTO;
using AKP_TrackManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace AKP_TrackManager.Interfaces
{
    public interface IAccidentRepository
    {
        Task<IEnumerable<Accident>> Index(int? page, string contextUserName, bool isAdmin);
        Task<IEnumerable<Accident>> IndexFilterAdmin(int? page, string contextUserName);
        Task<AccidentCarMemberDto> Details(int? id, string contextUserName, bool isAdmin);
        SelectList GetMembersSelectList();
        SelectList GetRegPlatesSelectList();
        SelectList GetSelectedMemberSelectList(int memberId);
        Task<AccidentCarMemberDto> Create(AccidentCarMemberDto accident);
        Task<AccidentCarMemberDto> Edit(int? id, string contextUserName, bool isAdmin);
        Task<AccidentCarMemberDto> Edit(int id, AccidentCarMemberDto accident, string contextUserName, bool isAdmin);
        Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin);
        Task<AccidentCarMemberDto> Delete(int? id, string contextUserName, bool isAdmin);
        bool AccidentExists(int id);
    }
}
