using Microsoft.EntityFrameworkCore;
using Scales.ReferenceBook.Domain.Entities;
using Scales.ReferenceBook.Domain.Extensions;

namespace Scales.ReferenceBook.Domain
{
    public class ReferenceBookDbContext : DbContext
    {
        public ReferenceBookDbContext(DbContextOptions<ReferenceBookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("ReferenceBook");

            modelBuilder.Entity<ReferenceTransport>()
                .HasQueryFilter(x => x.IsDeleted == false);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}
