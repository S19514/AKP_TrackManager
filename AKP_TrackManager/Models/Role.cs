using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace AKP_TrackManager.Models
{
    public partial class Role
    {
        public Role()
        {
            Members = new HashSet<Member>();
        }

        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
