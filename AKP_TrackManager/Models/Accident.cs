using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Accident
    {
        public Accident()
        {
            CarAccidentByMembers = new HashSet<CarAccidentByMember>();
        }

        public int AccidentId { get; set; }
        [Required]
        public DateTime AccidentDate { get; set; }
        public int Severity { get; set; }
        public bool AnyoneInjured { get; set; }

        public virtual ICollection<CarAccidentByMember> CarAccidentByMembers { get; set; }
    }
}
