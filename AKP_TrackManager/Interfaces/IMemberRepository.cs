using AKP_TrackManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKP_TrackManager.Interfaces
{
    public interface IMemberRepository
    {
        Task<IEnumerable<Member>> Index();
        Task<IActionResult> Details(int? id);
        IActionResult Create();
        Task<IActionResult> Create(Member member);
        Task<IActionResult> Edit(int? id);
        Task<IActionResult> Edit(int id, Member member);
        Task<IActionResult> Delete(int? id);
        Task<IActionResult> DeleteConfirmed(int id);
        bool MemberExists(int id);
    }
}
