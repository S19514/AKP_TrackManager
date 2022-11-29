using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int ClubMembershipMembershipId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int MemberMemberId { get; set; }

        public virtual ClubMembership ClubMembershipMembership { get; set; }
        public virtual Member MemberMember { get; set; }
    }
}
