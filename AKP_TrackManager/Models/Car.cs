using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        public decimal EngingeCapacity { get; set; }
        public int EnginePower { get; set; }
        [Required]
        public string RegPlate { get; set; }

        public virtual ICollection<CarAccidentByMember> CarAccidentByMembers { get; set; }
        public virtual ICollection<CarMember> CarMembers { get; set; }
        public virtual ICollection<MemberCarOnLap> MemberCarOnLaps { get; set; }
    }
}
