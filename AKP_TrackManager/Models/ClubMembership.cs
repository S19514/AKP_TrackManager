using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class ClubMembership
    {
        public ClubMembership()
        {
            Payments = new HashSet<Payment>();
        }

        public int MembershipId { get; set; }
        public int JoinDate { get; set; }
        public decimal FeeAmount { get; set; }
        public int MemberMemberId { get; set; }

        public virtual Member MemberMember { get; set; } = null!;
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
