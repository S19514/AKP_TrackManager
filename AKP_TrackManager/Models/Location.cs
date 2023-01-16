using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Location
    {
        public Location()
        {
            training = new HashSet<training>();
        }

        public int LocationId { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Invalid {0} field length({2}-{1})")]
        public string FriendlyName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Invalid {0} field length({2}-{1})")]
        public string Town { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Invalid {0} field length({2}-{1})")]
        public string Street { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Invalid {0} field length({2}-{1})")]
        public string Country { get; set; }

        public virtual ICollection<training> training { get; set; }
    }
}
