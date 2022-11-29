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
        public string FriendlyName { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string Street { get; set; }
        public string Country { get; set; }

        public virtual ICollection<training> training { get; set; }
    }
}
