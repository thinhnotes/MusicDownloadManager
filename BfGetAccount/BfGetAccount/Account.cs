using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BfGetAccount
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Site> Sites { get; set; }
    }
}
