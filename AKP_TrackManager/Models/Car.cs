﻿using System;
using System.Collections.Generic;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Car
    {
        public Car()
        {
            CarAccidentByMembers = new HashSet<CarAccidentByMember>();
            CarMembers = new HashSet<CarMember>();
            MemberCarOnLaps = new HashSet<MemberCarOnLap>();
        }

        public int CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal EngingeCapacity { get; set; }
        public int EnginePower { get; set; }

        public virtual ICollection<CarAccidentByMember> CarAccidentByMembers { get; set; }
        public virtual ICollection<CarMember> CarMembers { get; set; }
        public virtual ICollection<MemberCarOnLap> MemberCarOnLaps { get; set; }
    }
}