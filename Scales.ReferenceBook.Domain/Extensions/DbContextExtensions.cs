using Microsoft.EntityFrameworkCore;
using Scales.ReferenceBook.Domain.Entities;

namespace Scales.ReferenceBook.Domain.Extensions
{
    public static class DbContextExtensions
    {
        public static void OnBeforeSaving(this DbContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    var now = DateTime.UtcNow;

                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            baseEntity.LastModifiedDate = now;
                            break;

                        case EntityState.Added:
                            baseEntity.CreatedDate = now;
                            baseEntity.LastModifiedDate = now;
                            break;
                    }
                }
            }
        }
    }
}
