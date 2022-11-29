using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace AKP_TrackManager.Models.DTO
{
    public class AccidentCarMemberDto
    {
        public int MemberId { get; set; }
        public int CarId { get; set; }
        public string EmailAddress { get; set; }
        public string RegPlate { get; set; }
        public int AccidentId { get; set; }
        public DateTime AccidentDate { get; set; }
        public int Severity { get; set; }
        public bool AnyoneInjured { get; set; }

        public virtual ICollection<CarAccidentByMember> CarAccidentByMembers { get; set; }
    }
}
