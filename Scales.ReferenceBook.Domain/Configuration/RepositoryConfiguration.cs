using Microsoft.Extensions.DependencyInjection;
using Scales.ReferenceBook.Domain.Repositories.Implementations;
using Scales.ReferenceBook.Domain.Repositories.Interfaces;

namespace Scales.ReferenceBook.Domain.Configuration
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReferenceTransportRepository, ReferenceTransportRepository>();
            services.AddScoped<UnitOfWork>();
        }
    }
}
