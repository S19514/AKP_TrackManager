using System;
using System.ComponentModel.DataAnnotations;

namespace AKP_TrackManager.Models.DTO
{
    public class MemberCarOnLapDto : Member
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

    }
}