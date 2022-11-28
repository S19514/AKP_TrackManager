using System;
using System.Collections.Generic;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class ClubMembership
    {
        public ClubMembership()
        {
            Payments = new HashSet<Payment>();
        }

        public int MembershipId { get; set; }
        public DateTime JoinDate { get; set; }
        public decimal FeeAmount { get; set; }
        public int MemberMemberId { get; set; }

        public virtual Member MemberMember { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
