using System;
using System.Collections.Generic;

namespace AKP_TrackManager.Models
{
    public partial class Location
    {
        public Location()
        {
            training = new HashSet<training>();
        }

        public int LocationId { get; set; }
        public string Town { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<training> training { get; set; }
    }
}
