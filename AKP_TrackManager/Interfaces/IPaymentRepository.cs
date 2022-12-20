using AKP_TrackManager.Models;
using AKP_TrackManager.Models.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKP_TrackManager.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> Index(int? page, string contextUserName, bool isAdmin);
        Task<IEnumerable<Payment>> IndexFilterAdmin(int? page, string contextUserName);
        Task<Payment> Details(int? id, string contextUserName, bool isAdmin);
        SelectList GetMembershipsSelectList();
        SelectList GetSelectedMembershipSelectList(int membershipId);
        SelectList GetMembersSelectList();
        SelectList GetSelectedMemberSelectList(int memberId);
        Task<Payment> Create(Payment payment);
        Task<bool> DeleteConfirmed(int id, string contextUserName, bool isAdmin);
        bool PaymentExists(int id);
    }
}
