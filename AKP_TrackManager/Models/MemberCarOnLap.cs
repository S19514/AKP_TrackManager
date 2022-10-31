using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class MemberCarOnLap
    {
        public int MemberLapId { get; set; }
        public int MemberMemberId { get; set; }
        public int CarCarId { get; set; }
        public int LapLapId { get; set; }

        public virtual Car CarCar { get; set; } = null!;
        public virtual Lap LapLap { get; set; } = null!;
        public virtual Member MemberMember { get; set; } = null!;
    }
}
