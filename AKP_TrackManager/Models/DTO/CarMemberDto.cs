using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKP_TrackManager.Models.DTO
{
    public class CarMemberDto
    {
        public int CarId { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public decimal EngingeCapacity { get; set; }
        [Required]
        public int EnginePower { get; set; }
        [Required]
        public string RegPlate { get; set; }
        public int MemberId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
  
        public string EmailAddress { get; set; }
        public bool IsAscendant { get; set; }       
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
