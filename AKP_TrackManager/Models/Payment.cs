using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int ClubMembershipMembershipId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int MemberMemberId { get; set; }

        public virtual ClubMembership ClubMembershipMembership { get; set; } = null!;
        public virtual Member MemberMember { get; set; } = null!;
    }
}
