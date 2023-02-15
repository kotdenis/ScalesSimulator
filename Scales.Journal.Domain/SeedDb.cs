using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Scales.Journal.Domain
{
    public static class SeedDb
    {
        public static async Task PopulateAsync(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<JournalDbContext>();
            context.Database.Migrate();
            await Task.CompletedTask;
        }
    }
}
