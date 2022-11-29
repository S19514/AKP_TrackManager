using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKP_TrackManager.Models.DTO
{
    public class MemberCarOnLapDto
    {
        public int CarId { get; set; }
     
        public string Make { get; set; }
        
        public string Model { get; set; }
        [Required]
        public decimal EngingeCapacity { get; set; }
        [Required]
        public int EnginePower { get; set; }        
        public string RegPlate { get; set; }
        //LAP
        
        public int LapId { get; set; }
        [Required]
        public TimeSpan MeasuredTime { get; set; }
        [Required]
        public TimeSpan PenaltyTime { get; set; }
        [Required]
        public TimeSpan AbsoluteTime { get; set; }
        [Required]
        public int TrainingTrainingId { get; set; }

        public DateTime TrainingDate { get; set; }
        public string TrainingLocationString { get; set; }
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