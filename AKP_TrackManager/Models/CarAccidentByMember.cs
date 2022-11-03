using System;
using System.Collections.Generic;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class CarAccidentByMember
    {
        public int CarAccidentMemberId { get; set; }
        public int MemberMemberId { get; set; }
        public int CarCarId { get; set; }
        public int AccidentAccidentId { get; set; }

        public virtual Accident AccidentAccident { get; set; }
        public virtual Car CarCar { get; set; }
        public virtual Member MemberMember { get; set; }
    }
}
