using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BfGetAccount
{
    public class SiteUser
    {
        [Key, Column(Order = 0)]
        public int SiteId { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }

        public virtual Site Site { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Password")]
        public int? PasswordId { get; set; }

        public Password Password { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int CountCheck { get; set; }
        public List<int> ListPassChecked { get; set; }
        public string Dynamic { get; set; }
    }
}