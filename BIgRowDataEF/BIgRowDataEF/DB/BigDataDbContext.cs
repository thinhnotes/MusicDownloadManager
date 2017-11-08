using System.Data.Entity;
using BigRowDataEF.Entity;

namespace BIgRowDataEF.DB
{
    public class BigDataDbContext : DbContext
    {
        public BigDataDbContext()
            : base("BigData")
        {
            
        }

        public DbSet<TaxCategory> TaxCategories { get; set; }
    }
}