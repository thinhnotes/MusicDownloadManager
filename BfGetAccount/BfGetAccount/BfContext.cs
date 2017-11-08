using System.Data.Entity;

namespace BfGetAccount
{
    public class BfContext : DbContext
    {
        public BfContext()
            : base("Name=DFAccount")
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteUser> SiteUsers { get; set; }
        public DbSet<Password> Passwords { get; set; }
    }
}