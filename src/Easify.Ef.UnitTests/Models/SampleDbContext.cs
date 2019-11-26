using Easify.Http;
using Microsoft.EntityFrameworkCore;

namespace Easify.Ef.UnitTests.Models
{
    public class SampleDbContext : DbContextBase
    {
        public SampleDbContext(DbContextOptions options, IOperationContext operationContext) : base(options, operationContext)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SampleEntity>();
        }
    }
}