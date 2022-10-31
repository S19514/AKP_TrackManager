using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class CarAccidentByMember
    {
        public int CarAccidentMemberId { get; set; }
        public int MemberMemberId { get; set; }
        public int CarCarId { get; set; }
        public int AccidentAccidentId { get; set; }

        public virtual Accident AccidentAccident { get; set; } = null!;
        public virtual Car CarCar { get; set; } = null!;
        public virtual Member MemberMember { get; set; } = null!;
    }
}
