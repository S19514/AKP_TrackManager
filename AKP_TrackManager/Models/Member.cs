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
        [StringLength(150,MinimumLength = 3,ErrorMessage ="Invalid name length(3-150)")]
        public string Name { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Invalid surname length(3-150)")]
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        [StringLength(20, MinimumLength = 9, ErrorMessage = "Invalid phone number length(9-20)")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Invalid phone number length(6-100)")]
        public string EmailAddress { get; set; }
        public bool IsAscendant { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Invalid password length(5-100)")]

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
