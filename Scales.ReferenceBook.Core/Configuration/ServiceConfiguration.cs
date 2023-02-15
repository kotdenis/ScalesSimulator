using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scales.ReferenceBook.Core.Options;
using Scales.ReferenceBook.Core.Services.Implementations;
using Scales.ReferenceBook.Core.Services.Interfaces;
using Scales.ReferenceBook.Core.Validation;
using SharedLibrary.DTO.ReferenceBook;
using StackExchange.Redis;

namespace Scales.ReferenceBook.Core.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RefTransportDto>, RefTransportValidator>();
            services.AddScoped<IRefTransportService, RefTransportService>();
            services.AddHostedService<RabbitBackgroundWorker>();
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
