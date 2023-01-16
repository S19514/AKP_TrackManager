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
        [Range(0.0, Double.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public decimal Length { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Invalid {0} field length({2}-{1})")]
        public string PresetName { get; set; }
        [Required]
        [Range(0,2147483647, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int PresetNumber { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "Invalid {0} field length({2}-{1})")]

        public string PresetImageLink { get; set; }

        public virtual ICollection<training> training { get; set; }
    }
}
