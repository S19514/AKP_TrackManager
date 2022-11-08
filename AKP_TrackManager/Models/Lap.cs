using System;
using System.Collections.Generic;

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
        public TimeSpan AbsoluteTime { get; set; }
        public int TrainingTrainingId { get; set; }

        public virtual Training TrainingTraining { get; set; }
        public virtual ICollection<MemberCarOnLap> MemberCarOnLaps { get; set; }
    }
}
