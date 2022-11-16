using System;
using System.Collections.Generic;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class TrackConfiguration
    {
        public TrackConfiguration()
        {
            training = new HashSet<training>();
        }

        public int TrackId { get; set; }
        public bool Reversable { get; set; }
        public decimal Length { get; set; }
        public string PresetName { get; set; }
        public int PresetNumber { get; set; }
        public string PresetImageLink { get; set; }

        public virtual ICollection<training> training { get; set; }
    }
}
