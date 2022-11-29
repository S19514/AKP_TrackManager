using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public bool Reversable { get; set; }
        [Required]
        public decimal Length { get; set; }
        [Required]
        public string PresetName { get; set; }
        [Required]
        public int PresetNumber { get; set; }
        [Required]
        public string PresetImageLink { get; set; }

        public virtual ICollection<training> training { get; set; }
    }
}
