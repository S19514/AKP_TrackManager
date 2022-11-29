using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Lap
    {
        public Lap()
        {
            MemberCarOnLaps = new HashSet<MemberCarOnLap>();
        }

        public int LapId { get; set; }
        public TimeSpan MeasuredTime { get; set; }
        public TimeSpan PenaltyTime { get; set; }
        [Required]
        public TimeSpan AbsoluteTime { get; set; }
        public int TrainingTrainingId { get; set; }

        public virtual training TrainingTraining { get; set; }
        public virtual ICollection<MemberCarOnLap> MemberCarOnLaps { get; set; }
    }
}
