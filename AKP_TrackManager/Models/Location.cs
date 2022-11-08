using System;
using System.Collections.Generic;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Location
    {
        public Location()
        {
            training = new HashSet<Training>();
        }

        public int LocationId { get; set; }
        public string Town { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public virtual ICollection<Training> training { get; set; }
    }
}
