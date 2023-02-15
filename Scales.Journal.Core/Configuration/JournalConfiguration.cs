using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scales.Journal.Core.Infrastructure;
using Scales.Journal.Core.Options;
using Scales.Journal.Core.Services.Implementations;
using Scales.Journal.Core.Services.Interfaces;
using Scales.Journal.Core.Validation;
using SharedLibrary.DTO.Journal;
using StackExchange.Redis;

namespace Scales.Journal.Core.Configuration
{
    public static class JournalConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IJournalService, JournalService>();
            services.AddScoped<IValidator<TransportDto>, TransportValidator>();
            services.AddTransient<IWeighingSimulator, WeighingSimulator>();
        }

        public static void AddRedisConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RedisConnection");
            var multiplexer = ConnectionMultiplexer.Connect(connectionString);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
        }

        public static void AddRabbitMqConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.RabbitMq));
        }
    }
}
