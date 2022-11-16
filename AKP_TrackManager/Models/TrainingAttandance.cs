using System;
using System.Collections.Generic;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class TrainingAttandance
    {
        public int TrainingAttandanceId { get; set; }
        public int TrainingTrainingId { get; set; }
        public int? MemberMemberId { get; set; }

        public virtual Member MemberMember { get; set; }
        public virtual training TrainingTraining { get; set; }
    }
}
