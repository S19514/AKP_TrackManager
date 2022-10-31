using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class training
    {
        public training()
        {
            Laps = new HashSet<Lap>();
            TrainingAttandances = new HashSet<TrainingAttandance>();
        }

        public int TrainingId { get; set; }
        public int TrackConfigurationTrackId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int LocationLocationId { get; set; }

        public virtual Location LocationLocation { get; set; } = null!;
        public virtual TrackConfiguration TrackConfigurationTrack { get; set; } = null!;
        public virtual ICollection<Lap> Laps { get; set; }
        public virtual ICollection<TrainingAttandance> TrainingAttandances { get; set; }
    }
}
