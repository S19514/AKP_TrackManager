using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class Lap
    {
        public Lap()
        {
            MemberCarOnLaps = new HashSet<MemberCarOnLap>();
        }

        public int LapId { get; set; }
        public int MeasuredTime { get; set; }
        public int PenaltyTime { get; set; }
        public int AbsoluteTime { get; set; }
        public int TrainingTrainingId { get; set; }

        public virtual training TrainingTraining { get; set; } = null!;
        public virtual ICollection<MemberCarOnLap> MemberCarOnLaps { get; set; }
    }
}
