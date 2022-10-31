using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class CarMember
    {
        public int MemberMemberId { get; set; }
        public int CarCarId { get; set; }
        public int CarMemberId { get; set; }

        public virtual Car CarCar { get; set; } = null!;
        public virtual Member MemberMember { get; set; } = null!;
    }
}
