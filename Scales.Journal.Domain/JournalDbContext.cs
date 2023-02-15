using Microsoft.EntityFrameworkCore;
using Scales.Journal.Domain.Entities;
using Scales.Journal.Domain.Extensions;

namespace Scales.Journal.Domain
{
    public class JournalDbContext : DbContext
    {
        public JournalDbContext(DbContextOptions<JournalDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("Journal");

            modelBuilder.Entity<Transport>()
                .HasMany(x => x.Axles)
                .WithOne(x => x.Transport);
            modelBuilder.Entity<Transport>()
                .HasQueryFilter(x => x.IsDeleted == false);

            modelBuilder.Entity<Axles>()
                .HasOne(x => x.Transport)
                .WithMany(x => x.Axles);
            modelBuilder.Entity<Axles>()
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
