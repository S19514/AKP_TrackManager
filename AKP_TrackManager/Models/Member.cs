using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Member
    {
        public Member()
        {
            CarAccidentByMembers = new HashSet<CarAccidentByMember>();
            CarMembers = new HashSet<CarMember>();
            ClubMemberships = new HashSet<ClubMembership>();
            MemberCarOnLaps = new HashSet<MemberCarOnLap>();
            Payments = new HashSet<Payment>();
            TrainingAttandances = new HashSet<TrainingAttandance>();
        }

        public int MemberId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        [Required] 
        public string EmailAddress { get; set; }
        public bool IsAscendant { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsStudent { get; set; }
        public int RoleRoleId { get; set; }
        public bool IsBlocked { get; set; }

        public virtual Role RoleRole { get; set; }
        public virtual ICollection<CarAccidentByMember> CarAccidentByMembers { get; set; }
        public virtual ICollection<CarMember> CarMembers { get; set; }
        public virtual ICollection<ClubMembership> ClubMemberships { get; set; }
        public virtual ICollection<MemberCarOnLap> MemberCarOnLaps { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<TrainingAttandance> TrainingAttandances { get; set; }
    }
}
