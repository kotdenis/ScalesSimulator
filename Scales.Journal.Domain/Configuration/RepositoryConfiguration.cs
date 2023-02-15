using Microsoft.Extensions.DependencyInjection;
using Scales.Journal.Domain.Repositories.Implementations;
using Scales.Journal.Domain.Repositories.Interfaces;

namespace Scales.Journal.Domain.Configuration
{
    public static class RepositoryConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITransportRepository, TransportRepository>();
            services.AddScoped<IAxlesRepository, AxlesRepository>();
            services.AddScoped<UnitOfWork>();
        }
    }
}
